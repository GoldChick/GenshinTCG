namespace TCGBase
{
    public abstract class AbstractCardPersistent
    {
        /// <summary>
        /// 用于指示材质，没有namespace则默认置为"minecraft"
        /// </summary>
        public virtual string TextureNameSpace { get => "Minecraft"; }
        /// <summary>
        /// Persistent的NameID用于指示材质<br/>
        /// 对于Support和Summon，材质在action文件夹中获得<br/>
        /// 对于Effect，材质在icon文件夹中获得<br/><br/>
        /// <b>如果没有检测到材质，则不会显示该Persistent</b>
        /// </summary>
        public virtual string TextureNameID { get => "nullable"; }
        /// <summary>
        /// 产生时候的基础使用次数，默认和[最大次数]一样
        /// </summary>
        public virtual int InitialUseTimes { get => MaxUseTimes; }
        public abstract int MaxUseTimes { get; }
        /// <summary>
        /// team: team me<br/>
        /// persistent: this buff<br/>
        /// 通过此方式结算伤害时，对角色index的描述皆为绝对坐标，并且均为单体伤害<br/>
        /// 在#Api.Persistent.PersistentTriggerl#中提供一些预设，如刷新次数，清除，黄盾，紫盾等
        /// </summary>
        public abstract PersistentTriggerDictionary TriggerDic { get; }
        /// <summary>
        /// 可用次数为0时是否立即删除(表现为记active为false，下次/本次结算完毕后清除)<br/>
        /// 为false时，需要自己手动控制AbstractPersistent.Active，每次结算(update())会清除所有deactive的effect
        /// </summary>
        public virtual bool DeleteWhenUsedUp { get => true; }
        /// <summary>
        /// 用来给客户端提供使用的表现参数
        /// </summary>
        public virtual int[] Info(AbstractPersistent p) => new int[] { p.AvailableTimes };
    }
}
