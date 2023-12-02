﻿using System.Text.Json.Serialization;

namespace TCGBase
{
    /// <summary>
    /// 描述可增减的花费<br/>
    /// 不过请不要直接操作属性来加减dice数量，因为有特殊规则<br/>
    /// 考虑使用<see cref="CostModifier{T}"/> where T : AbstractUseDiceSender
    /// </summary>
    public class CostVariable : AbstractVariable
    {
        protected int[] _costs;
        /// <summary>
        /// 同色 冰水火雷岩草风 杂色<br/>
        /// 请不要通过此属性修改
        /// </summary>
        public int[] DiceCost { get => _costs; }
        public int MPCost { get; protected set; }
        public CostVariable()
        {
            _costs = new int[9];
        }
        public CostVariable(CostInit init) : this(init.DiceCost, init.MPCost)
        {

        }
        [JsonConstructor]
        public CostVariable(int[] dicecost, int mpcost) : this()
        {
            for (int i = 0; i < 9 && i < dicecost.Length; i++)
            {
                _costs[i] = dicecost[i];
            }
            MPCost = mpcost;
        }
        /// <summary>
        /// 判断提供的dices是否能<b>恰好</b>满足需要
        /// </summary>
        /// <param name="supply">选中的元素骰，万能 冰水火雷岩草风</param>
        public bool DiceEqualTo(int[]? supply)
        {
            supply ??= new int[9];
            if (supply.All(p => p >= 0) && supply.Sum() == _costs.Sum())
            {
                if (_costs[0] > 0)
                {
                    //同色
                    int num = supply.Where(i => i > 0).Count();
                    return num <= 1 || (num == 2 && supply.ElementAtOrDefault(0) > 0);
                }
                else
                {
                    //杂色+某(几)种指定颜色
                    //只需要满足 总数量相同+万能能够满足缺少的元素
                    return supply[0] >= _costs.Select((c, index) => c -= int.Min(c, supply.ElementAtOrDefault(index))).ToArray()[1..8].Sum();
                }
            }
            return false;
        }
    }
    public enum DiceModifierType
    {
        Same,
        Cryo,
        Hydro,
        Pyro,
        Electro,
        Geo,
        Dendro,
        Anemo,
        Void
    }

    public class CostModifier
    {
        protected DiceModifierType Type { get; }
        protected int Num { get; set; }
        public CostModifier(DiceModifierType type, int num)
        {
            Type = type;
            Num = num;
        }
        public bool Modifier(CostVariable dcv)
        {
            bool act = true;

            if (dcv.DiceCost[0] > 0)
            {
                if (Type == DiceModifierType.Same || Num <= 0)
                {
                    dcv.DiceCost[(int)Type] -= int.Min(dcv.DiceCost[(int)Type], Num);
                }
                else
                {
                    act = false;
                }
            }
            //否则 abcd颜色+x杂色
            else if (Type == DiceModifierType.Same)
            {
                //[有色骰]消耗只能减少不能增加
                if (dcv.DiceCost.Any(i => i > 0))
                {
                    if (Num >= 0)
                    {
                        int a = Num;
                        for (int i = 1; i < 9; i++)
                        {
                            if (a == 0)
                            {
                                break;
                            }
                            int min = int.Min(dcv.DiceCost[i], a);
                            a -= min;
                            dcv.DiceCost[i] -= min;
                        }
                    }
                }
                else if (Num != 0)
                {
                    act = false;
                }
            }
            else if (Type == DiceModifierType.Void)
            {
                if (dcv.DiceCost[8] > 0 || Num <= 0)
                {
                    dcv.DiceCost[8] -= int.Min(dcv.DiceCost[8], Num);
                }
                else
                {
                    act = false;
                }
            }
            else
            {
                if (dcv.DiceCost[(int)Type] > 0 || Num <= 0)
                {
                    var min = int.Min(dcv.DiceCost[(int)Type], Num);
                    Num -= min;
                    dcv.DiceCost[(int)Type] -= min;

                    min = int.Min(dcv.DiceCost[8], Num);
                    Num -= min;
                    dcv.DiceCost[8] -= min;
                }
                else if (dcv.DiceCost[8] > 0)
                {
                    var min = int.Min(dcv.DiceCost[8], Num);
                    Num -= min;
                    dcv.DiceCost[8] -= min;
                }
                else
                {
                    act = false;
                }
            }

            return act;
        }
    }
    public class CostModifier<T> : CostModifier where T : AbstractUseDiceSender
    {
        public Func<PlayerTeam, AbstractPersistent, T, int>? DynamicNum { get; }
        public CostModifier(DiceModifierType type, int num) : base(type, num)
        {
        }
        public CostModifier(DiceModifierType type, Func<PlayerTeam, AbstractPersistent, T, int>? dynamicNum) : base(type, 0)
        {
            DynamicNum = dynamicNum;
        }
        public bool Modifier(PlayerTeam me, AbstractPersistent p, T uds, CostVariable dcv)
        {
            Num = DynamicNum?.Invoke(me, p, uds) ?? Num;
            return Modifier(dcv);
        }
    }
    /// <summary>
    /// 为卡牌、技能构建CostInit的中间态，经过处理后得到最终结果<br/><br/>
    /// 特殊规则<b>需要[同色]时，不能再需要[杂色]或[元素骰]</b>
    /// </summary>
    public class CostCreate
    {
        protected int[] _costs;
        /// <summary>
        /// 同色 冰水火雷岩草风 杂色
        /// </summary>
        internal int[] DiceCost { get => _costs; }
        /// <summary>
        /// 对于[技能]，直接填写数值即可<br/>
        /// 对于[卡牌]，需要实现<see cref="IEnergyConsumerCard"/>，指定额外Target的中角色的index（天赋卡默认index=0），否则默认为出战角色
        /// </summary>
        internal int MPCost { get; private protected set; }
        public CostCreate()
        {
            _costs = new int[9];
        }
        public CostInit ToCostInit()
        {
            if (_costs[0] > 0)
            {
                for (int i = 1; i < 9; i++)
                {
                    _costs[i] = 0;
                }
            }
            return new(_costs, MPCost);
        }
        public CostCreate Void(int num)
        {
            _costs[8] += num;
            return this;
        }
        public CostCreate Same(int num)
        {
            _costs[0] += num;
            return this;
        }
        public CostCreate Cryo(int num)
        {
            _costs[1] += num;
            return this;
        }
        public CostCreate Hydro(int num)
        {
            _costs[2] += num;
            return this;
        }
        public CostCreate Pyro(int num)
        {
            _costs[3] += num;
            return this;
        }
        public CostCreate Electro(int num)
        {
            _costs[4] += num;
            return this;
        }
        public CostCreate Geo(int num)
        {
            _costs[5] += num;
            return this;
        }
        public CostCreate Dendro(int num)
        {
            _costs[6] += num;
            return this;
        }
        public CostCreate Anemo(int num)
        {
            _costs[7] += num;
            return this;
        }
        public CostCreate MP(int num)
        {
            MPCost += num;
            return this;
        }
    }
    /// <summary>
    /// 创建[费用]，<see cref="CostCreate"/><br/><br/>
    /// 特殊规则<b>需要[同色]时，不能再需要[杂色]或[元素骰]</b>
    /// </summary>
    public class CostInit : CostCreate
    {
        public CostInit() : base()
        {
        }
        internal CostInit(int[] dicecost, int mpcost) : base()
        {
            for (int i = 0; i < 9 && i < dicecost.Length; i++)
            {
                _costs[i] = dicecost[i];
            }
            MPCost = mpcost;
        }
        /// <summary>
        ///  0 同色(有同色则其他骰都为0)<br/>
        ///  1-7 冰水火雷岩草风<br/>
        ///  8 杂色<br/>
        ///  9 充能<br/>
        /// </summary>
        /// <returns></returns>
        public int[] GetCost() => DiceCost.Append(MPCost).ToArray();
        public CostVariable ToCostVariable() => new(this);
    }
}
