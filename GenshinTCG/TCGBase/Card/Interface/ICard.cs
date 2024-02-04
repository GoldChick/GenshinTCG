namespace TCGBase
{
    public interface ICard
    {
        /// <summary>
        /// 对于[角色牌][行动牌]，hidden的不能被选择；<br/>
        /// 对于[角色/出战状态]，hidden的前台不显示
        /// 对于[召唤物]，hidden无用
        /// </summary>
        public bool Hidden { get; }
        /// <summary>
        /// 产生时候的基础使用次数，默认和[最大次数]一样
        /// </summary>
        public int InitialUseTimes { get; }
        public int MaxUseTimes { get; }
        /// <summary>
        /// 用来标识是变种。0为默认种<br/>
        /// -1为武器 -2为圣遗物 -3为天赋 -4为计数器 -5为支援区<br/>
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
        public PersistentTriggerableList TriggerableList { get; }

        public void Update<T>(PlayerTeam me, Persistent<T> persistent) where T : AbstractCardBase;
    }
}
