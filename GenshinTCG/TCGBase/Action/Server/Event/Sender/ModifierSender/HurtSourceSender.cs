namespace TCGBase
{
    public class HurtSourceSender : SimpleSender, IMaySkillSupplier, IPeristentSupplier, IModifier
    {
        public Persistent Source { get; }
        public AbstractTriggerable Triggerable { get; }
        ISkillable? IMaySkillSupplier.MaySkill => Triggerable as ISkillable;
        Persistent IPeristentSupplier.Persistent => Source;
        //TODO:“假”伤害，用于伤害预览
        public bool RealAction => true;

        internal HurtSourceSender(int sourceTeamID, Persistent source, AbstractTriggerable triggerable) : base(sourceTeamID)
        {
            Source = source;
            Triggerable = triggerable;
        }
    }
}
