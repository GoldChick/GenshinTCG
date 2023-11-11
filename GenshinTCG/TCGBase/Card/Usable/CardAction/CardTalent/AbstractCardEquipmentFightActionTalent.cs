namespace TCGBase
{
    public abstract class AbstractCardEquipmentFightActionTalent : AbstractCardEquipmentOnlyTalent
    {
        /// <summary>
        /// 所属的skill的index，AfterUseAction()中默认会使用一次这个技能(如果不是被动)
        /// </summary>
        public abstract int Skill { get; }
        /// <summary>
        /// 默认实现为先带上天赋，然后如果天赋不是被动技能的，就释放一次<b>原来拥有的</b><see cref="Skill"/>号技能
        /// </summary>
        public override void AfterUseAction(PlayerTeam me, int[] targetArgs)
        {
            base.AfterUseAction(me, targetArgs);
            var c = me.Characters[targetArgs[0]];
            var index = Skill % c.Card.Skills.Length;
            if (c.Card.Skills[index] is not AbstractPassiveSkill)
            {
                me.Game.HandleEvent(new NetEvent(new NetAction(ActionType.UseSKill, index)), me.TeamIndex);
            }
        }
        /// <summary>
        /// 默认实现为需要是本人的天赋，并且为被动技能/该角色在前台
        /// </summary>
        public override sealed bool CanBeUsed(PlayerTeam me, int[] targetArgs)
        {
            var c = me.Characters[targetArgs[0]];
            var sks = c.Card.Skills;
            return base.CanBeUsed(me, targetArgs) && targetArgs[0] == me.CurrCharacter && sks[Skill % sks.Length] is not AbstractPassiveSkill && c.Active;
        }
    }
}
