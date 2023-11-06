namespace TCGBase
{
    public abstract class AbstractCardTalent : AbstractCardAction, ITargetSelector
    {
        public override bool CostSame => false;
        /// <summary>
        /// 绑定在角色身上的effect，在这里覆写角色技能
        /// </summary>
        public abstract CardPersistentTalent Effect { get; }
        /// <summary>
        /// 所属的角色的nameid
        /// TODO: without namespace currently
        /// </summary>
        public abstract string CharacterNameID { get; }
        public override string NameID => $"talent_{CharacterNameID}";
        /// <summary>
        /// 所属的skill的index，AfterUseAction()中默认会使用一次这个技能(如果不是被动)
        /// </summary>
        public abstract int Skill { get; }
        /// <summary>
        /// 默认实现为先带上天赋，然后如果天赋不是被动技能的，就释放一次<b>原来拥有的</b>技能
        /// </summary>
        public override void AfterUseAction(PlayerTeam me, int[]? targetArgs = null)
        {
            var c = me.Characters[targetArgs[0]];
            c.Talent.TryRemoveAt(0);
            c.Talent.Add(new(Effect));
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
            var sks = me.Characters[targetArgs[0]].Card.Skills;
            return me.Characters[targetArgs[0]].Card.NameID == CharacterNameID && (targetArgs[0] == me.CurrCharacter || sks[Skill % sks.Length] is AbstractPassiveSkill);
        }
        public TargetEnum[] TargetEnums => new TargetEnum[] { TargetEnum.Character_Me };
    }
}
