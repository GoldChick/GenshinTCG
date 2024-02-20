namespace TCGBase
{
    internal enum DiceModifierType
    {
        Void,
        Color,
        Any,
        If
    }
    public class DiceModifierSender : AbstractSender, IMaySkillSupplier, IPeristentSupplier
    {
        public override string SenderName => DiceModType.ToString();
        internal DiceModifierType DiceModType { get; set; }
        public bool RealAction { get; }
        /// <summary>
        /// 打出卡牌时，是牌；使用技能时，是对应角色；切换角色时，是当前角色
        /// </summary>
        public Persistent Persistent { get; }
        public ICostable Source { get; }
        ISkillable? IMaySkillSupplier.MaySkill => Source as ISkillable;

        internal DiceModifierSender(int teamID, Persistent card, AbstractCardAction cardAction, bool realAction) : base(teamID)
        {
            Persistent = card;
            Source = cardAction;
            RealAction = realAction;
        }
        internal DiceModifierSender(int teamID, Character character, ICostable skill, bool realAction) : base(teamID)
        {
            Persistent = character;
            Source = skill;
            RealAction = realAction;
        }
        internal DiceModifierSender(int teamID, Character currcharacter, bool realAction) : base(teamID)
        {
            Persistent = currcharacter;
            Source = new SwitchCost();
            RealAction = realAction;
        }
    }
    public class SwitchCost : ICostable
    {
        public CostInit Cost => new CostCreate().Add(ElementCategory.Void, 1).ToCostInit();
    }
}
