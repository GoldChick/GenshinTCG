using TCGBase;

namespace TCGBase
{
    public abstract class AbstractPassiveSkill : AbstractCardSkill
    {
        public override sealed SkillCategory Category => SkillCategory.P;
        public override sealed int[] Costs => Array.Empty<int>();
        public override sealed bool CostSame => false;
        public override sealed bool Prepare => false;
        public override sealed bool GiveMP => false;
        /// <summary>
        /// 表明将在什么时候触发这个被动<br/>
        /// 由于被动技能可以在后台触发，因此targetargs[0]表示该角色在队伍中的位置<br/>
        /// targetargs[1]表示teamid
        /// </summary>
        public abstract string[] TriggerDic { get; }
        /// <summary>
        /// 为true时，只会以任意方式触发一次
        /// </summary>
        public abstract bool TriggerOnce { get; }
    }
}