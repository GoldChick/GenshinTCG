namespace TCGBase
{
    public interface ICardPersistnet
    {
        /// <summary>
        /// 注册时给出，仅限行动牌、角色牌、状态，默认为"minecraft"<br/>
        /// 也可以选择不注册，自己给出namespace
        /// </summary>
        public string Namespace { get; }
        /// <summary>
        /// 卡牌的nameID(a-z+0-9),如"keqing114"
        /// </summary>
        public string NameID { get; }
        //TODO:是对的吗

        /// <summary>
        /// 产生时候的基础使用次数，默认和[最大次数]一样
        /// </summary>
        public int InitialUseTimes { get; }
        public int MaxUseTimes { get; }
        /// <summary>
        /// 用来标识是变种。-1为默认种（当然你可以从0开始）<br/>
        /// 标号大于等于0的变种会得到不同的文本(TODO:这个在客户端暂时还没有实现)<br/>
        /// <br/><b>
        /// 给一个区域重复添加同一个Persistent时，如果是variant相同并且旧的为active，就调用更新Update() ; 如果variant不同，就先删除再添加
        /// </b>
        /// </summary>
        public int Variant { get; }
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
        public void Update<T>(Persistent<T> persistent) where T : ICardPersistnet;
        public int[] Info(AbstractPersistent p);
    }
    public abstract class AbstractCardPersistent : AbstractCardBase, ICardPersistnet
    {
        public virtual int InitialUseTimes { get => MaxUseTimes; }
        public abstract int MaxUseTimes { get; }
        public virtual bool CustomDesperated { get => false; }
        public virtual int Variant { get => -1; }
        public abstract PersistentTriggerDictionary TriggerDic { get; }
        public virtual void Update<T>(Persistent<T> persistent) where T : ICardPersistnet => persistent.AvailableTimes = int.Max(persistent.AvailableTimes, MaxUseTimes);
        /// <summary>
        /// 用来给客户端提供使用的表现参数
        /// </summary>
        public virtual int[] Info(AbstractPersistent p) => new int[] { p.AvailableTimes };
    }
}
