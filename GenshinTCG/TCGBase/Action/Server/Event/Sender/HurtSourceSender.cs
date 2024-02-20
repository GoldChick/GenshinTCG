namespace TCGBase
{
    public class HurtSourceSender : AbstractSender, IMaySkillSupplier, IPeristentSupplier
    {
        public override string SenderName => ModifierName.ToString();
        public SenderTag ModifierName { get; internal set; }
        public Persistent Source { get; }
        public AbstractTriggerable Triggerable { get; }
        ISkillable? IMaySkillSupplier.MaySkill => Triggerable as ISkillable;
        Persistent IPeristentSupplier.Persistent => Source;

        internal HurtSourceSender(SenderTag tag, int sourceTeamID, Persistent source, AbstractTriggerable triggerable) : base(sourceTeamID)
        {
            ModifierName = tag;
            Source = source;
            Triggerable = triggerable;
        }
    }
}
