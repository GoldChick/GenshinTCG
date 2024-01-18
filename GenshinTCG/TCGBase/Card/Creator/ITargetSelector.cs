namespace TCGBase
{
    /// <summary>
    /// 供Event使用的TargetEnum，创建<see cref="TargetEnum"/>
    /// </summary>
    internal enum TargetEnumForNetEvent
    {
        //Card_Enemy,
        //Card_Me,
        Character_Enemy,
        Character_Me,

        Dice_Optional,
        Card_Optional,

        Summon_Enemy,
        Summon_Me,
        Support_Enemy,
        Support_Me
    }
    /// <summary>
    /// 供创建使用
    /// </summary>
    public enum TargetEnum
    {
        //Card_Enemy,
        //Card_Me,
        Character_Enemy,
        Character_Me,

        Summon_Enemy,
        Summon_Me,
        Support_Enemy,
        Support_Me
    }
    /// <summary>
    /// 实现这个接口的卡牌使用时除了骰子，还必须需要选择另外的目标<br/>
    /// 由于各种原因，只允许给[事件牌] [装备牌]使用
    /// </summary>
    internal interface ITargetSelector
    {
        /// <summary>
        /// 将按照顺序依次选取<br/>
        /// 如:[诸武精通]:{Character_Me,Character_Me}<br/>
        /// [送你一程]:{Summon}<br/>
        /// </summary>
        public TargetDemand[] TargetDemands { get; }
    }
    public class TargetDemand
    {
        public TargetEnum Target { get; }
        public Func<PlayerTeam, int[], bool> Condition { get; }
        /// <summary>
        /// <b>Action:</b><br/>
        /// PlayerTeam: 表示我方队伍情况<br/>
        /// int[]: 表示已经选中的Target对应的参数，第n个demand能获得参数的length为n<br/>
        /// </summary>
        public TargetDemand(TargetEnum target, Func<PlayerTeam, int[], bool> condition)
        {
            Target = target;
            Condition = condition;
        }
    }
    internal class TargetValid
    {
        public TargetEnumForNetEvent Target { get; }
        public IEnumerable<int> Indexs { get; }
        internal TargetValid(TargetEnum target, IEnumerable<int> indexs)
        {
            Target = target.ToNetEvent();
            Indexs = indexs;
        }
        internal TargetValid(TargetEnumForNetEvent target, IEnumerable<int> indexs)
        {
            Target = target;
            Indexs = indexs;
        }
    }
}
