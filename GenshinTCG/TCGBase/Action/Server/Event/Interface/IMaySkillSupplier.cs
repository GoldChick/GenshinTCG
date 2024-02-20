namespace TCGBase
{
    /// <summary>
    /// 提供给Condition判断是否是技能引发的[伤害]、[费用]等
    /// </summary>
    internal interface IMaySkillSupplier
    {
        public ISkillable? MaySkill { get; }
    }
}
