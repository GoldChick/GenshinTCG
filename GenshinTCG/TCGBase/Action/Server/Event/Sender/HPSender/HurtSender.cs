namespace TCGBase
{
    /// <summary>
    /// 受到伤害后产生的sender，可能并不一定用于直接触发<br/>
    /// </summary>
    public class HurtSender : NoDamageHurtSender
    {
        public int Damage { get; internal set; }//set只是为了合并
        internal HurtSender(int teamID, DamageVariable dv, ReactionTags reaction, DamageSource directSource, IDamageSource rootSource, int initialElement) : base(teamID, dv.Element,dv.TargetIndex, reaction, directSource, rootSource, initialElement)
        {
            Damage = dv.Damage;
        }
    }
}
