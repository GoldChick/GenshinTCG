namespace TCGBase
{
    public abstract class AbstractSomeVariable : AbstractVariable
    {
        //给子类用...
        protected int _amount;
        public int Amount { get => _amount; set => _amount = int.Max(0, _amount); }
        public int TargetTeam { get; }
        public DamageSource Direct { get; private set; }
        /// <summary>
        /// 目标角色的绝对坐标
        /// </summary>
        public int TargetIndex { get; private set; }
        protected private AbstractSomeVariable(int targetTeam, DamageSource direct, int targetIndex)
        {
            TargetTeam = targetTeam;
            Direct = direct;
            TargetIndex = targetIndex;
        }
    }
    public class ElementVariable : AbstractSomeVariable
    {
        public ReactionTags Reaction { get; internal set; }
        public DamageElement Element { get; set; }
        public ElementVariable(int targetTeam, DamageElement element, DamageSource direct, int targetIndex) : base(targetTeam, direct, targetIndex)
        {
            Element = element;
        }
    }
    public class DamageSomeVariable : ElementVariable
    {
        public DamageSomeVariable(int targetTeam, DamageElement element, int amount, DamageSource direct, int targetIndex) : base(targetTeam, element, direct, targetIndex)
        {
            Amount = amount;
        }
    }
    public class HealSomeVariable : AbstractSomeVariable
    {
        public HealSomeVariable(int targetTeam, int amount, DamageSource direct, int targetIndex) : base(targetTeam, direct, targetIndex)
        {
        }
    }
}

