namespace TCGBase
{
    internal interface ICardPersistnet
    {
        /// <summary>
        /// 用于指示材质，没有namespace则默认置为"minecraft"
        /// </summary>
        public string TextureNameSpace { get; }
        /// <summary>
        /// Persistent的NameID用于指示材质<br/>
        /// 对于Support和Summon，材质在action文件夹中获得<br/>
        /// 对于Effect，材质在icon文件夹中获得<br/><br/>
        /// <b>如果没有检测到材质，则不会显示该Persistent</b>
        /// </summary>
        public string TextureNameID { get; }
        /// <summary>
        /// 产生时候的基础使用次数，默认和[最大次数]一样
        /// </summary>
        public int InitialUseTimes { get; }
        public int MaxUseTimes { get; }

        /// <summary>
        /// 是否自定义0可用次数时候的清除<br/>
        /// 为false时，可用次数为0时会使AbstractPersistent.Active为false，下次/本次结算完毕后清除<br/>
        /// 为true时，需要自己手动控制AbstractPersistent.Active，每次结算(update())会清除所有deactive的effect。<br/>
        /// <b>为true时改变可用次数会重新让action=true</b>
        /// </summary>
        public bool CustomDesperated { get; }
        /// <summary>
        /// team: team me<br/>
        /// persistent: this buff<br/>
        /// 通过此方式结算伤害时，对角色index的描述皆为绝对坐标，并且均为单体伤害<br/>
        /// 在#Api.Persistent.PersistentTriggerl#中提供一些预设，如刷新次数，清除，黄盾，紫盾等
        /// </summary>
        public PersistentTriggerDictionary TriggerDic { get; }
    }
    public abstract class AbstractCardPersistent : ICardPersistnet
    {
        public virtual string TextureNameSpace { get => "Minecraft"; }
        public virtual string TextureNameID { get => "nullable"; }
        public virtual int InitialUseTimes { get => MaxUseTimes; }
        public abstract int MaxUseTimes { get; }
        public virtual bool CustomDesperated { get => false; }
        public abstract PersistentTriggerDictionary TriggerDic { get; }

        /// <summary>
        /// 重复刷新[召唤物]/[状态]的时候会如何行动，默认为取[当前次数]和[最大次数]的最大值
        /// </summary>
        public virtual void Update<T>(Persistent<T> persistent) where T : AbstractCardPersistent
        {
            persistent.AvailableTimes = int.Max(persistent.AvailableTimes, MaxUseTimes);
        }
        /// <summary>
        /// 用来给客户端提供使用的表现参数
        /// </summary>
        public virtual int[] Info(AbstractPersistent p) => new int[] { p.AvailableTimes };
    }
}
