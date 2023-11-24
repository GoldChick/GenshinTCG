namespace TCGBase
{
    /// <summary>
    /// 实现这个接口的卡牌(一般为[事件牌]、[装备牌])打出时需要对能量有限制<br/>
    /// 技能其实不用实现了，直接写好了（确信）
    /// </summary>
    public interface IEnergyConsumer
    {
        public int MPNum { get; }
        /// <summary>
        /// 消耗的充能来源的角色对应的index在AdditionalArgs中的index
        /// </summary>
        public int MPCharacterIndexInAdditionalTargetArgs { get; }
    }
}
