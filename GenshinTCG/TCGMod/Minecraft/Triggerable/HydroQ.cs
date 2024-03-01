using TCGBase;

namespace Minecraft
{
    /// <summary>
    /// 根据Target Count增伤，可选增加倍率mul、各自乘以可用次数mulAvailableTimes
    /// </summary>
    public class HydroQ : AbstractTriggerable, IWhenThenAction
    {
        public override string NameID { get => "hydroq"; protected set { } }
        public override string Tag => SenderTag.DamageIncrease.ToString();
        public TargetRecord Target { get; }
        public int Mul { get; }
        public bool MulAvailableTimes { get; }
        public int Consume { get; }
        public List<ConditionRecordBase> When { get; }
        public HydroQ(int mul = 1, int consume = 0, bool mulAvailableTimes = false, TargetRecord? target = null, List<ConditionRecordBase>? when = null)
        {
            Mul = mul;
            Consume = consume;
            MulAvailableTimes = mulAvailableTimes;
            Target = target ?? new();
            When = when ?? new();
        }
        /// <summary>
        /// 不考虑CurrCharacter为-1
        /// </summary>
        public override void Trigger(PlayerTeam me, Persistent persitent, AbstractSender sender, AbstractVariable? variable)
        {
            if ((this as IWhenThenAction).IsConditionValid(me, persitent, sender, variable) && variable is DamageVariable dv)
            {
                if (MulAvailableTimes)
                {
                    dv.Amount += Target.GetTargets(me, persitent, sender, variable, out _).Select(p => p.AvailableTimes).Sum() * Mul;
                }
                else
                {
                    dv.Amount += Target.GetTargets(me, persitent, sender, variable, out _).Count * Mul;
                }
                persitent.AvailableTimes -= Consume;
            }
        }
    }
}
