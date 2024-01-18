namespace TCGBase
{
    public interface IEnergyConsumerCard
    {
        /// <summary>
        /// 消耗的充能来源的角色对应的index在AdditionalArgs中的index<br/>
        /// 只对Card有效
        /// </summary>
        public int CostMPFromCharacterIndexInArgs { get; }
    }
}
