namespace TCGBase
{
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
        /// 通过此方式结算伤害时，对角色index的描述皆为绝对坐标，并且均为单体伤害<br/>
        /// 在#Api.Persistent.PersistentTriggerl#中提供一些预设，如刷新次数，清除，黄盾，紫盾等
        /// </summary>
        public PersistentTriggerDictionary TriggerDic { get; }
        public void Update<T>(PlayerTeam me, Persistent<T> persistent) where T : ICardPersistent;
        public void OnDesperated(PlayerTeam me, int region);
    }
    public abstract class AbstractCardPersistent : AbstractCardBase, ICardPersistent, IDamageSource
    {
        /// <summary>
        /// 默认为addition，表示[旋火轮]等上来不直接来源于角色，但仍然是我方造成的伤害<br/>
        /// 可以继承改为nowhere，表示自己莫名其妙受到的伤害，以此实现不能被对方增伤加伤
        /// </summary>
        public abstract DamageSource DamageSource { get; }
        public virtual int InitialUseTimes { get => MaxUseTimes; }
        public abstract int MaxUseTimes { get; }
        public virtual bool CustomDesperated { get => false; }
        public int Variant { get; protected set; }
        public abstract PersistentTriggerDictionary TriggerDic { get; }
        private protected AbstractCardPersistent()
        {
        }
        public virtual void Update<T>(PlayerTeam me, Persistent<T> persistent) where T : ICardPersistent
        {
            persistent.Data = null;
            persistent.AvailableTimes = int.Max(persistent.AvailableTimes, MaxUseTimes);
        }
        public virtual void OnDesperated(PlayerTeam me, int region)
        {
        }
    }
    public abstract class AbstractCardSummon : AbstractCardPersistent
    {
        public sealed override DamageSource DamageSource => DamageSource.Summon;
    }
    public abstract class AbstractCardEffect : AbstractCardPersistent
    {
        public sealed override DamageSource DamageSource => DamageSource.NoWhere;
    }
}
