namespace TCGBase
{
    /// <summary>
    /// 准备技能
    /// </summary>
    public abstract class AbstractPrepareSkill : AbstractCardSkill
    {
        public override sealed SkillCategory Category => SkillCategory.P;
        public override sealed int[] Costs => Array.Empty<int>();
        public override sealed bool CostSame => false;
        public override sealed bool Prepare => false;
        public override sealed bool GiveMP => false;
    }
}