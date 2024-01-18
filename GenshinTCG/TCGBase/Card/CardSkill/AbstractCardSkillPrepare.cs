namespace TCGBase
{
    public abstract class AbstractCardSkillPrepare: AbstractCardSkill
    {
        public override int GiveMP => 0;
        public override bool TriggerAfterUseSkill =>false;
    }
}
