namespace TCGBase
{
    /// <summary>
    /// 用于处理加费减费问题<br/>
    /// 需要注意的是加费减费有[实际触发]和[模拟触发]两种情况
    /// </summary>
    public class PersistentDiceCostModifier<T> : PersistentTrigger where T : AbstractUseDiceSender
    {
        private readonly Func<PlayerTeam, AbstractPersistent, T, AbstractVariable?, bool> _condition;
        private readonly CostModifier<T> _modifier;
        private readonly bool _costafteruse;
        private readonly Action<PlayerTeam, AbstractPersistent, T>? _action;
        public PersistentDiceCostModifier(Func<PlayerTeam, AbstractPersistent, T, AbstractVariable?, bool> condition, int element, Func<PlayerTeam, AbstractPersistent, T, int>? dynamicNum, bool costaftertrigger = true, Action<PlayerTeam, AbstractPersistent, T>? optionalActionIfTrigger = null)
        {
            _condition = condition;
            element = int.Clamp(element, -1, 7);
            element = (element + 9) % 9;
            _modifier = new((DiceModifierType)element, dynamicNum);

            _costafteruse = costaftertrigger;
            _action = optionalActionIfTrigger;
        }
        /// <summary>
        /// 减费成功（指确认的行动之后、消耗骰子之前的判定）<br/>
        /// element:-1=杂色骰 0=有效骰 1-7=冰水火雷岩草风
        /// </summary>
        public PersistentDiceCostModifier(Func<PlayerTeam, AbstractPersistent, T, AbstractVariable?, bool> condition, int element, int num, bool costaftertrigger = true, Action<PlayerTeam, AbstractPersistent, T>? optionalActionIfTrigger = null)
        {
            _condition = condition;
            element = int.Clamp(element, -1, 7);
            element = (element + 9) % 9;
            _modifier = new((DiceModifierType)element, num);

            _costafteruse = costaftertrigger;
            _action = optionalActionIfTrigger;
        }
        /// <summary>
        /// 需要available才能减费<br/>
        /// 对teamid默认无要求<br/>
        /// 仅当[减费]且[指定元素]不满足的时候，认为是[没有触发] TODO: check it
        /// </summary>
        public override void Trigger(PlayerTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
        {
            if (persitent.AvailableTimes > 0 && sender is T uds && _condition.Invoke(me, persitent, uds, variable) && variable is CostVariable dcv)
            {
                if (_modifier.Modifier(me, persitent, uds, dcv) && uds.IsRealAction)
                {
                    if (_costafteruse)
                    {
                        persitent.AvailableTimes--;
                    }
                    _action?.Invoke(me, persitent, uds);
                }
            }
        }
    }
}
