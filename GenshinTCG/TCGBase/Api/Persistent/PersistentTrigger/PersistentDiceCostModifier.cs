namespace TCGBase
{
    /// <summary>
    /// 用于处理加费减费问题<br/>
    /// 需要注意的是加费减费有[实际触发]和[模拟触发]两种情况
    /// </summary>
    public class PersistentDiceCostModifier<T> : PersistentTrigger where T : AbstractUseDiceSender
    {
        private readonly Func<PlayerTeam, AbstractPersistent, T, AbstractVariable?, bool> _condition;
        private readonly Func<PlayerTeam, AbstractPersistent, T, int>? _dynamicNum;
        private readonly int _element;
        private readonly int _num;
        private readonly bool _costafteruse;
        private readonly Action<PlayerTeam, AbstractPersistent, T>? _action;
        /// <summary>
        /// 减费动态个骰子成功
        /// </summary>
        public PersistentDiceCostModifier(Func<PlayerTeam, AbstractPersistent, T, AbstractVariable?, bool> condition, int element, Func<PlayerTeam, AbstractPersistent, T, int>? dynamicNum, bool costaftertrigger = true, Action<PlayerTeam, AbstractPersistent, T>? optionalActionIfTrigger = null)
        {
            _condition = condition;
            _element = int.Clamp(element, -1, 7);
            _dynamicNum = dynamicNum;
            _num = 0;
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
            _element = int.Clamp(element, -1, 7);
            _num = num;
            _dynamicNum = null;
            _costafteruse = costaftertrigger;
            _action = optionalActionIfTrigger;
        }
        /// <summary>
        /// 需要available才能减费<br/>
        /// 对teamid默认无要求
        /// </summary>
        public override void Trigger(PlayerTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
        {
            //TODO:不知道对于负数会有什么反应

            //TODO: 0不再是杂色
            if (persitent.AvailableTimes > 0 && sender is T uds)
            {
                if (_condition.Invoke(me, persitent, uds, variable) && variable is CostVariable dcv)
                {
                    int num = _dynamicNum?.Invoke(me, persitent, uds) ?? _num;

                    bool act = true;

                    if (_element > 0)
                    {
                        int a = num;
                        int min = int.Min(dcv.DiceCost[_element], a);
                        if (min >= 0)
                        {
                            a -= min;
                            dcv.DiceCost[_element] -= min;
                        }
                        else if (dcv.DiceCost[0] == 0)
                        {
                            act = false;
                        }
                        dcv.DiceCost[0] -= int.Min(dcv.DiceCost[0], a);
                    }
                    else if (_element == 0)
                    {
                        if (dcv.DiceCost.Sum() == 0)
                        {
                            act = false;
                        }
                        else
                        {
                            int a = num;
                            for (int i = 1; i < 8; i++)
                            {
                                if (a == 0)
                                {
                                    break;
                                }
                                int min = int.Min(dcv.DiceCost[i], a);
                                if (min > 0)
                                {
                                    a -= min;
                                    dcv.DiceCost[i] -= min;
                                }
                            }
                            if (a > 0)
                            {
                                int min = int.Min(dcv.DiceCost[0], a);
                                dcv.DiceCost[0] -= min;
                            }
                        }
                    }
                    else if (_element == -1 && dcv.DiceCost[0] > 0)
                    {
                        dcv.DiceCost[0] -= int.Min(dcv.DiceCost[0], num);
                    }
                    else
                    {
                        act = false;
                    }

                    if (uds.IsRealAction && act)
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
}
