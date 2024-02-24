﻿namespace TCGBase
{
    /*
     *                     Sender => Persistent
     * AfterUseSkillSender => 使用技能的角色
     * HurtSourceSender   => 作为来源的Persistent+(要)受到伤害的角色
     * DiceModifierSender => 作为费用来源的Persistent
     */
    public enum TargetType
    {
        //从出战角色开始向右
        Character,
        Summon,
        Support,
        //↓下面对于Select没用↓
        //本身的Persistent
        This,
        //本身是角色状态，则添加对应的角色
        Owner,
        //从(有些)sender获取对应Persistent
        Sender,
        //仅用于添加状态，此时不调用GetTargets()
        Team
    }
    public record class TargetRecord : SelectRecord
    {
        /// <summary>
        /// 默认为0，即第一个；可置为负表示全部
        /// </summary>
        public int Index { get; }
        /// <summary>
        /// 是否逆序TargetList，默认否，对Custom无效
        /// </summary>
        public bool Reverse { get; }

        public TargetRecord(TargetType type = TargetType.Character, int index = 0, bool reverse = false, TargetTeam team = TargetTeam.Me, List<ConditionRecordBase>? when = null) : base(type, team, when)
        {
            Index = index;
            Reverse = reverse;
        }

        public List<Persistent> GetTargets(PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v, out PlayerTeam team)
        {
            var localteam = Team == TargetTeam.Enemy ? me.Enemy : me;
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
                case TargetType.Sender:
                    if (s is IPeristentSupplier ips)
                    {
                        targets.Add(ips.Persistent);
                    }
                    if (s is IMulPersistentSupplier imps)
                    {
                        targets.AddRange(imps.Persistents);
                    }
                    if (v is DamageVariable dv)
                    {
                        targets.Add(team.Game.Teams[dv.TargetTeam].Characters[dv.TargetIndex]);
                    }
                    break;
                case TargetType.This:
                    targets.Add(p);
                    break;
                case TargetType.Owner:
                    if (team.Characters.ElementAtOrDefault(p.PersistentRegion) is Character c)
                    {
                        targets.Add(c);
                    }
                    break;
            }
            if (Reverse)
            {
                targets.Reverse();
            }
            if (Index >= 0 && Index < targets.Count)
            {
                targets = new() { targets[Index] };
            }
            return targets.Where(pe => (this as IWhenThenAction).IsConditionValid(localteam, pe, s, v)).ToList();
        }
    }
}
