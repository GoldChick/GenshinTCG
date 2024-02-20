namespace TCGBase
{
    /// <summary>
    /// 供Event使用的TargetEnum，创建<see cref="TargetEnum"/>
    /// </summary>
    internal enum TargetEnumForNetEvent
    {
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
        Character_Enemy,
        Character_Me,
        Summon_Enemy,
        Summon_Me,
        Support_Enemy,
        Support_Me
    }
    public interface ITargetSelector
    {
        public List<TargetDemand> TargetDemands { get; }
    }
    public class TargetDemand
    {
        public TargetTeam Team { get; }
        public TargetType Select { get; }
        public Func<PlayerTeam, IEnumerable<Persistent>, Persistent, bool> Condition { get; }
        public TargetDemand(TargetTeam team, TargetType select, Func<PlayerTeam, IEnumerable<Persistent>, Persistent, bool> condition)
        {
            Team = team;
            Select = select;
            Condition = condition;
        }
        internal TargetDemand(SelectRecord target)
        {
            Team = target.Team;
            Select = target.Type;
            Condition = (me, ps, newp) => target.When.TrueForAll(condition => condition.Valid(me, newp, new ActionDuringUseCardSender(me.TeamIndex, ps), null));
        }
        internal Persistent? GetPersistent(PlayerTeam team, int index)
        {
            var t = Team == TargetTeam.Enemy ? team.Enemy : team;
            return Select switch
            {
                TargetType.Character => t.Characters.ElementAtOrDefault(index),
                TargetType.Summon => t.Summons.ElementAtOrDefault(index),
                TargetType.Support => t.Supports.ElementAtOrDefault(index),
                _ => throw new Exception("IsManyTargetDemandValid():不支持的SelectType!")
            };
        }
        internal TargetEnum ToEnum()
        {
            return (TargetEnum)((int)Team + 2 * (int)Select);
        }
        internal bool IsPersistentValid(PlayerTeam team, int index, List<Persistent> old)
        {
            var t = Team == TargetTeam.Enemy ? team.Enemy : team;
            var persistent = Select switch
            {
                TargetType.Character => t.Characters.ElementAtOrDefault(index),
                TargetType.Summon => t.Summons.ElementAtOrDefault(index),
                TargetType.Support => t.Supports.ElementAtOrDefault(index),
                _ => throw new Exception("IsManyTargetDemandValid():不支持的SelectType!")
            };
            if (persistent != null && Condition.Invoke(t, old, persistent))
            {
                old.Add(persistent);
                return true;
            }
            return false;
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
