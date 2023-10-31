namespace TCGBase
{
    /// <summary>
    /// 受到伤害后产生的sender，可能并不一定用于直接触发<br/>
    /// </summary>
    public class HurtSender : AbstractSender
    {
        public override string SenderName => SenderTag.AfterHurt.ToString();
        public int Element { get; init; }
        public int Damage { get; internal set; }//set只是为了合并
        public int TargetIndex { get; init; }
        public ReactionTags Reaction { get; init; }
        internal HurtSender(int teamID, DamageVariable dv, ReactionTags reaction) : base(teamID)
        {
            Element = dv.Element;
            Damage = dv.Damage;
            TargetIndex = dv.TargetIndex;
            Reaction = reaction;
        }
    }
}
