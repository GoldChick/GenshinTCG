
namespace TCGBase
{
    internal enum DiceModifierType
    {
        Void,
        Color,
        Any,
        If
    }
    public class DiceModifierSender : AbstractSender, IMaySkillSupplier, IPeristentSupplier, IMulPersistentSupplier
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
        /// <summary>
        /// 切换角色时，表示目标角色
        /// </summary>
        public IEnumerable<Persistent> Persistents { get; }

        internal DiceModifierSender(int teamID, Persistent card, AbstractCardAction cardAction, bool realAction) : base(teamID)
        {
            Persistent = card;
            Source = cardAction;
            RealAction = realAction;
            Persistents = new List<Persistent>();
        }
        internal DiceModifierSender(int teamID, Character character, ICostable skill, bool realAction) : base(teamID)
        {
            Persistent = character;
            Source = skill;
            RealAction = realAction;
            Persistents = new List<Persistent>();
        }
        internal DiceModifierSender(int teamID, Character currcharacter, Character target, bool realAction) : base(teamID)
        {
            Persistent = currcharacter;
            Source = new SwitchCost();
            RealAction = realAction;
            Persistents = new List<Persistent>() { target };
        }
    }
    public class SwitchCost : ICostable
    {
        public CostInit Cost => new CostCreate().Add(ElementCategory.Void, 1).ToCostInit();
    }
}
