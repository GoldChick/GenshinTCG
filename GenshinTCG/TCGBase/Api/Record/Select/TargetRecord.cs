namespace TCGBase
{
    public enum TargetType
    {
        //从出战角色开始向右
        Character,
        Summon,
        Support,
        //↓下面对于Select没用↓
        //需要自己清楚的知道参数来源于什么，With对此不生效
        Custom,
        //本身的Persistent
        This,
        //仅用于添加状态，此时不调用GetTargets()
        Team
    }
    public record class TargetRecord : SelectRecord
    {
        /// <summary>
        /// 默认为0，即第一个
        /// </summary>
        public int Index { get; }
        /// <summary>
        /// 是否逆序TargetList，默认否，对Custom无效
        /// </summary>
        public bool Reverse { get; }

        public TargetRecord(TargetType type = TargetType.Character, int index = 0, bool reverse = false, DamageTargetTeam team = DamageTargetTeam.Me, List<ConditionRecordBase>? with = null) : base(type, team, with)
        {
            Index = index;
            Reverse = reverse;
        }

        public List<Persistent> GetTargets(PlayerTeam me, Persistent p, AbstractSender s, out PlayerTeam team)
        {
            var localteam = Team == DamageTargetTeam.Enemy ? me.Enemy : me;
            team = localteam;
            List<Persistent> targets = new();
            switch (Type)
            {
                case TargetType.Character:
                    if (team.CurrCharacter == -1)
                    {
                        targets.AddRange(team.Characters);
                    }
                    else
                    {
                        for (int i = 0; i < team.Characters.Length; i++)
                        {
                            targets.Add(team.Characters[(i + team.CurrCharacter) % team.Characters.Length]);
                        }
                    }
                    break;
                case TargetType.Summon:
                    targets.AddRange(team.Summons);
                    break;
                case TargetType.Support:
                    targets.AddRange(team.Supports);
                    break;
                case TargetType.This:
                    targets.Add(p);
                    break;
                case TargetType.Custom:
                    if (s is ITargetSupplier supplier)
                    {
                        targets.Add(supplier.GetTarget(team, Index));
                        return targets;
                    }
                    break;
            }
            targets = targets.Where(pe => With.All(condition => condition.Not ^ condition.GetPersistentPredicate().Invoke(localteam, pe))).ToList();
            if (Reverse)
            {
                targets.Reverse();
            }
            if (Index >= 0 && Index <= targets.Count)
            {
                return new() { targets[Index] };
            }
            return targets;
        }
    }
}
