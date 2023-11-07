namespace TCGBase
{
    public abstract class AbstractCardTalent : AbstractCardEquipment<CardPersistentTalent>
    {
        public override sealed string NameID => $"talent_{CharacterNameID}";
        public override bool CostSame => false;
        /// <summary>
        /// 不指定namespace，则和本身（这张卡）的一样
        /// TODO: do namespace
        /// </summary>
        public virtual string? CharacterNamespace { get => null; }
        /// <summary>
        /// 所属的角色的nameid
        /// </summary>
        public abstract string CharacterNameID { get; }
        /// <summary>
        /// 所属的skill的index，AfterUseAction()中默认会使用一次这个技能(如果不是被动)
        /// </summary>
        public abstract int Skill { get; }
        /// <summary>
        /// 默认实现为先带上天赋，然后如果天赋不是被动技能的，就释放一次<b>原来拥有的</b><see cref="Skill"/>号技能
        /// </summary>
        public override void AfterUseAction(PlayerTeam me, int[]? targetArgs = null)
        {
            me.AddEquipment(Effect, targetArgs[0]);

            var c = me.Characters[targetArgs[0]];
            var index = Skill % c.Card.Skills.Length;
            if (c.Card.Skills[index] is not AbstractPassiveSkill)
            {
                me.Game.HandleEvent(new NetEvent(new NetAction(ActionType.UseSKill, index)), me.TeamIndex);
            }
        }
        public override bool CanBeArmed()
        {
            return base.CanBeArmed();
        }
        /// <summary>
        /// 默认实现为需要是本人的天赋，并且为被动技能/该角色在前台
        /// </summary>
        public override sealed bool CanBeUsed(PlayerTeam me, int[]? targetArgs = null)
        {
            var card = me.Characters[targetArgs[0]].Card;
            var sks = card.Skills;
            return $"{CharacterNamespace ?? Namespace}:{CharacterNameID}".Equals($"{card.Namespace}:{card.NameID}") && (targetArgs[0] == me.CurrCharacter || sks[Skill % sks.Length] is AbstractPassiveSkill);
        }
    }
}
