namespace TCGBase
{
    /// <summary>
    /// 用于处理加费减费问题<br/>
    /// 需要注意的是加费减费有[实际触发]和[模拟触发]两种情况
    /// </summary>
    public class PersistentDiceCostModifier<T> : PersistentTrigger where T: AbstractUseDiceSender
    {
        private readonly Func<PlayerTeam, AbstractPersistent, T, AbstractVariable?, bool> _condition;
        private readonly int _element;
        private readonly int _num;
        private readonly bool _costafteruse;
        private readonly Action<PlayerTeam, AbstractPersistent, T, AbstractVariable?>? _action;
        /// <summary>
        /// 减费成功（指确认的行动之后、消耗骰子之前的判定）后是否消耗使用次数
        /// </summary>
        /// <summary>
        /// element:-1=杂色骰 0=有效骰 1-7=冰水火雷岩草风<br/>
        /// num:至少1
        /// </summary>
        public PersistentDiceCostModifier(Func<PlayerTeam, AbstractPersistent, T, AbstractVariable?, bool> condition, int element, int num,bool costaftertrigger=true,Action<PlayerTeam, AbstractPersistent, T, AbstractVariable?>? optionalActionIfTrigger=null)
        {
            _condition = condition;
            _element = int.Clamp(element, -1, 7);
            _num = int.Max(num, 1);
            _costafteruse= costaftertrigger;
            _action= optionalActionIfTrigger;
        }
        /// <summary>
        /// 需要available才能减费
        /// </summary>
        public override void Trigger(PlayerTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
        {
            if (me.TeamIndex == sender.TeamID && persitent.AvailableTimes>0 && sender is T uds)
            {
                if (_condition.Invoke(me, persitent, uds, variable) && variable is DiceCostVariable dcv)
                {
                    bool act = true;

                    if (_element > 0)
                    {
                        int a = _num;
                        int min = int.Min(dcv.Costs[_element], a);
                        if (min > 0)
                        {
                            a -= min;
                            dcv.Costs[_element] -= min;
                        }
                        else if (dcv.Costs[0] == 0)
                        {
                            act = false;
                        }
                        dcv.Costs[0] -= int.Min(dcv.Costs[0], a);
                    }
                    else if (_element == 0)
                    {
                        if (dcv.Costs.Sum() == 0)
                        {
                            act = false;
                        }
                        else
                        {
                            int a = _num;
                            for (int i = 1; i < 8; i++)
                            {
                                if (a == 0)
                                {
                                    break;
                                }
                                int min = int.Min(dcv.Costs[i], a);
                                if (min > 0)
                                {
                                    a -= min;
                                    dcv.Costs[i] -= min;
                                }
                            }
                            if (a > 0)
                            {
                                int min = int.Min(dcv.Costs[0], a);
                                dcv.Costs[0] -= min;
                            }
                        }
                    }
                    else if (_element == -1 && dcv.Costs[0] > 0)
                    {
                        dcv.Costs[0] -= int.Min(dcv.Costs[0], _num);
                    }
                    else
                    {
                        act = false;
                    }

                    if (uds.IsRealAction && _costafteruse && act)
                    {
                        persitent.AvailableTimes--;
                        _action?.Invoke(me,persitent,uds,variable);
                    }
                }
            }
        }
    }
}
