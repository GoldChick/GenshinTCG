namespace TCGBase
{
    public enum PersistentTag
    {
        /// <summary>
        /// 具有该Tag的<b>[角色状态]</b>，会在造成伤害的[扣血]时被检测，然后稍后触发<br/>
        /// 注意：需要搭配治疗使用，否则会产生0血活角色，引起难以预料的游戏bug！<br/>
        /// </summary>
        AntiDie,
        /// <summary>
        /// 具有该Tag的状态，被标记为护盾，用于[双岩共鸣]、[贯虹]的检测
        /// </summary>
        YellowShield,
    }
    public interface ICardPersistent
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
        /// <summary>
        /// 对于[角色/出战状态]是是否可见，对于[角色]是是否可选择，对于其他无意义<br/>
        /// 默认为false
        /// </summary>
        public bool Hidden { get; }
        /// <summary>
        /// 产生时候的基础使用次数，默认和[最大次数]一样
        /// </summary>
        public int InitialUseTimes { get; }
        public int MaxUseTimes { get; }
        /// <summary>
        /// 用来标识是变种。0为默认种<br/>
        /// -1为武器 -2为圣遗物 -3为天赋 -4为计数器<br/>
        /// 标号%10 不同的变种视作不同但无法共存的状态，会得到不同的文本，重复附属时会删除旧状态<br/>
        /// 标号%10 相同，但标号/10 不同的变种视作相同的状态，用来给染色召唤物提供不同的材质，重复附属时会直接刷新
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
        /// 在#Api.Persistent.PersistentTriggerl#中提供一些预设，如刷新次数，清除，黄盾，紫盾等
        /// </summary>
        public PersistentTriggerList TriggerList { get; }

        public void Update<T>(PlayerTeam me, Persistent<T> persistent) where T : ICardPersistent;
    }
    public abstract class AbstractCardPersistent : AbstractCardBase, ICardPersistent
    {
        public virtual int InitialUseTimes { get => MaxUseTimes; }
        public abstract int MaxUseTimes { get; }
        public virtual bool CustomDesperated { get => false; }
        public int Variant { get; protected set; }
        public CostInit Cost => new CostCreate().ToCostInit();

        protected private AbstractCardPersistent(BaseCardRecord record) : base(record)
        {
        }
        public virtual void Update<T>(PlayerTeam me, Persistent<T> persistent) where T : ICardPersistent
        {
            persistent.Data = null;
            persistent.AvailableTimes = int.Max(persistent.AvailableTimes, MaxUseTimes);
        }
    }
    //TODO: check it?
    public abstract class AbstractCardSummon : AbstractCardPersistent
    {
        protected private AbstractCardSummon(BaseCardRecord record) : base(record)
        {
        }
    }
    public abstract class AbstractCardEffect : AbstractCardPersistent
    {
        protected private AbstractCardEffect(BaseCardRecord record) : base(record)
        {
        }
    }
}
