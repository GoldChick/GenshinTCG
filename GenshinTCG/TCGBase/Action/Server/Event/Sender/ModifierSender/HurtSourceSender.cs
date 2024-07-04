namespace TCGBase
{
    public class HurtSourceSender : AbstractSender, IMaySkillSupplier, IPeristentSupplier, IModifier
    {
        public override string SenderName => ModifierName.ToString();
        public SenderTag ModifierName { get; internal set; }
        public Persistent Source { get; }
        public AbstractTriggerable Triggerable { get; }
        ISkillable? IMaySkillSupplier.MaySkill => Triggerable as ISkillable;
        Persistent IPeristentSupplier.Persistent => Source;
        //TODO:“假”伤害，用于伤害预览
        public bool RealAction => true;

        internal HurtSourceSender(SenderTag tag, int sourceTeamID, Persistent source, AbstractTriggerable triggerable) : base(sourceTeamID)
        {
            ModifierName = tag;
            Source = source;
            Triggerable = triggerable;
        }
    }
}
