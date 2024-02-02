namespace TCGBase
{
    /// <summary>
    /// 受到伤害后产生的sender，可能并不一定用于直接触发<br/>
    /// </summary>
    public class HurtSender : NoDamageHurtSender
    {
        public int Damage { get; init; }
        /// <summary>
        /// 在计算扣血时获得此属性<br/>
        /// 为true的伤害的结算将拥有[结算后置][队伍反转][队伍替代]三种特性<br/>
        /// Credit: Gold_Chick
        /// </summary>
        public bool Deadly { get; internal set; }
        internal HurtSender(int teamID, DamageVariable dv, ReactionTags reaction, DamageSource directSource, ITriggerable rootSource, int initialElement) : base(teamID, dv.Element, dv.TargetIndex, reaction, directSource, rootSource, initialElement)
        {
            Damage = dv.Damage;
        }
    }
}
