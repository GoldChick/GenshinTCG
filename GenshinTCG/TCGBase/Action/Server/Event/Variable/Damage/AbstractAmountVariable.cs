namespace TCGBase
{
    public abstract class AbstractAmountVariable : AbstractVariable, IPersistentIndirectSupplier
    {
        //给子类用...
        protected int _amount;
        public int Amount { get => _amount; set => _amount = int.Max(0, value); }
        /// <summary>
        /// 倍率，增伤(除也算)算完之后乘到Amount上
        /// </summary>
        public double Mul { get; set; }
        public int TargetTeam { get; }
        public DamageSource Direct { get; private set; }
        /// <summary>
        /// 目标角色的绝对坐标
        /// </summary>
        public int TargetIndex { get; private set; }

        protected private AbstractAmountVariable(int targetTeam, DamageSource direct, int targetIndex)
        {
            TargetTeam = targetTeam;
            Direct = direct;
            TargetIndex = targetIndex;
        }
        Persistent IPersistentIndirectSupplier.GetPersistent(PlayerTeam team) => team.Game.Teams[TargetTeam].Characters[TargetIndex];
    }
    public class ElementVariable : AbstractAmountVariable
    {
        public ReactionTags Reaction { get; internal set; }
        public DamageElement Element { get; set; }
        public ElementVariable(int targetTeam, DamageElement element, DamageSource direct, int targetIndex) : base(targetTeam, direct, targetIndex)
        {
            Element = element;
            Amount = -1;
        }
    }
    public class DamageVariable : ElementVariable
    {
        public bool Deadly { get; internal set; }
        public DamageVariable(int targetTeam, DamageElement element, int amount, DamageSource direct, int targetIndex) : base(targetTeam, element, direct, targetIndex)
        {
            Amount = amount;
        }
    }
    public class HealVariable : AbstractAmountVariable
    {
        public HealVariable(int targetTeam, int amount, DamageSource direct, int targetIndex) : base(targetTeam, direct, targetIndex)
        {
            Amount = amount;
        }
    }
}

