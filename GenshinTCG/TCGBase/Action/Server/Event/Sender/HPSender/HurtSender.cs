namespace TCGBase
{
    /// <summary>
    /// 受到伤害后产生的sender，可能并不一定用于直接触发<br/>
    /// </summary>
    public class HurtSender : NoDamageHurtSender
    {
        public int Damage { get; init; }
        internal HurtSender(int teamID, DamageVariable dv, ReactionTags reaction, DamageSource directSource) : base(teamID, dv.Element, dv.TargetIndex, reaction, directSource)
        {
            Damage = dv.Damage;
        }
    }
}
