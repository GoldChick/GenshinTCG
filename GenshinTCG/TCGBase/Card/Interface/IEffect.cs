namespace TCGBase
{
    /// <summary>
    /// 实现此接口的卡牌，作为角色状态、出战状态、召唤物使用
    /// </summary>
    public interface IEffect
    {
        public int MaxUseTimes { get; }
        /// <summary>
        /// 是否自定义0可用次数时候的清除<br/>
        /// 为false时，可用次数为0时会使AbstractPersistent.Active为false，下次/本次结算完毕后清除<br/>
        /// 为true时，需要自己手动控制AbstractPersistent.Active，每次结算(update())会清除所有deactive的effect。<br/>
        /// <b>为true时改变可用次数会重新让action=true</b>
        /// </summary>
        public bool CustomDesperated { get; }
        public void Update(PlayerTeam me, Persistent persistent);
    }
}
