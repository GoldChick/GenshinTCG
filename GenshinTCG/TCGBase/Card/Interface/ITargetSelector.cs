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
        public DamageTargetTeam Team { get; }
        public TargetType Select { get; }
        public Func<PlayerTeam, List<Persistent>, bool> Condition { get; }
        public TargetDemand(DamageTargetTeam team, TargetType select, Func<PlayerTeam, List<Persistent>, bool> condition)
        {
            Team = team;
            Select = select;
            Condition = condition;
        }
        internal TargetDemand(SelectRecord select)
        {
            Team = select.Team;
            Select = select.Type;
            //TODO : condition
        }
        internal Persistent? GetPersistent(PlayerTeam team, int index)
        {
            var t = Team == DamageTargetTeam.Enemy ? team.Enemy : team;
            return Select switch
            {
                TargetType.Character => t.Characters.ElementAtOrDefault(index),
                TargetType.Summon => t.Summons.ElementAtOrDefault(index),
                TargetType.Support => t.Supports.ElementAtOrDefault(index),
                _ => throw new Exception("IsManyTargetDemandValid():不支持的SelectType!")
            };
        }
        internal bool IsPersistentValid(PlayerTeam team, int index, List<Persistent> old)
        {
            var t = Team == DamageTargetTeam.Enemy ? team.Enemy : team;
            var persistent = Select switch
            {
                TargetType.Character => t.Characters.ElementAtOrDefault(index),
                TargetType.Summon => t.Summons.ElementAtOrDefault(index),
                TargetType.Support => t.Supports.ElementAtOrDefault(index),
                _ => throw new Exception("IsManyTargetDemandValid():不支持的SelectType!")
            };
            if (persistent != null)
            {
                old.Add(persistent);
                return Condition.Invoke(t, old);
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
