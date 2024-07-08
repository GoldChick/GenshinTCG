using System.Text.Json.Serialization;

namespace TCGBase
{
    /*
     *                     Sender => Persistent
     * AfterUseSkillSender => 使用技能的角色
     * HurtSourceSender   => 作为来源的Persistent+(要)受到伤害的角色
     * DiceModifierSender => 作为费用来源的Persistent
     * OnCharacterOnSender>入场的角色
     */
    public enum TargetType
    {
        Lua,
        //从出战角色开始向右
        Character,
        Summon,
        Support,
        //↓下面对于Select没用↓
        //从(有些)sender获取对应Persistent
        Sender,

        //本身的Persistent
        This,
        //本身是角色状态，则添加对应的角色
        Owner,
        //获取所有角色状态、出战状态（不进行排序）
        Effect,
        //仅用于添加状态，此时不调用GetTargets()
        Team,
        //仅用于转移状态
        Hand,
    }
    public enum CharacterSortType
    {
        None,
        Reverse,
        ClosestToEnemy,
        HPLostMost,
    }
    public record class TargetRecord : SelectRecord
    {
        /// <summary>
        /// 默认为0，即第一个；可置为负表示全部；超过.count()之后为空<br/>
        /// 对于Character,Summon,Effect,Support，先取When，再取Index<br/>
        /// 对于Sender，先取Index，再取When
        /// </summary>
        public int Index { get; }
        /// <summary>
        /// 仅对Character有效
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CharacterSortType SortBy { get; }
        public TargetRecord(TargetType type = TargetType.Character, int index = 0, CharacterSortType sortby = CharacterSortType.None, TargetTeam team = TargetTeam.Me) : base(type, team)
        {
            Index = index;
            SortBy = sortby;
        }
        /// <param name="team">对于sender,this,owner类型，team为me</param>
        public List<Persistent> GetTargets(PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v, out PlayerTeam team)
        {
            var localteam = Team == TargetTeam.Enemy ? me.Enemy : me;
            team = localteam;
            List<Persistent> targets = new();
            switch (Type)
            {
                case TargetType.Character:
                    List<Character> chars = new();
                    if (team.CurrCharacter == -1)
                    {
                        chars.AddRange(team.Characters);
                    }
                    else
                    {
                        for (int i = 0; i < team.Characters.Length; i++)
                        {
                            chars.Add(team.Characters[(i + team.CurrCharacter) % team.Characters.Length]);
                        }
                    }
                    switch (SortBy)
                    {
                        case CharacterSortType.Reverse:
                            chars.Reverse();
                            break;
                        case CharacterSortType.ClosestToEnemy:
                            chars.Clear();
                            switch (team.Enemy.CurrCharacter)
                            {
                                case 1:
                                    chars.Add(team.Characters[0]);
                                    chars.Add(team.Characters[1]);
                                    chars.Add(team.Characters[2]);
                                    break;
                                case 2:
                                    chars.Add(team.Characters[1]);
                                    chars.Add(team.Characters[0]);
                                    chars.Add(team.Characters[2]);
                                    break;
                                default:
                                    chars.Add(team.Characters[2]);
                                    chars.Add(team.Characters[0]);
                                    chars.Add(team.Characters[1]);
                                    break;
                            }
                            break;
                        case CharacterSortType.HPLostMost:
                            chars = chars.OrderByDescending(c => c.Card.MaxHP - c.HP).ToList();
                            break;
                    }
                    if (Index >= 0)
                    {
                        if (chars.Where(pe => (this as IWhenThenAction).IsConditionValid(localteam, pe, s, v)).ElementAtOrDefault(Index) is Character valid)
                        {
                            targets.Add(valid);
                        }
                    }
                    else
                    {
                        targets.AddRange(chars);
                    }
                    break;
                case TargetType.Summon:
                    targets.AddRange(team.Summons);
                    break;
                case TargetType.Support:
                    targets.AddRange(team.Supports);
                    break;
                case TargetType.Sender:
                    team = me;
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
                    if (Index >= 0)
                    {
                        var oldtargets = targets;
                        targets = new();
                        if (Index < oldtargets.Count)
                        {
                            targets.Add(oldtargets[Index]);
                        }
                    }
                    break;
                case TargetType.This:
                    team = me;
                    targets.Add(p);
                    break;
                case TargetType.Owner:
                    team = me;
                    if (team.Characters.ElementAtOrDefault(p.PersistentRegion) is Character c)
                    {
                        targets.Add(c);
                    }
                    break;
                case TargetType.Effect:
                    for (int i = 0; i < team.Characters.Length; i++)
                    {
                        targets.AddRange(team.Characters[(i + team.CurrCharacter) % team.Characters.Length].Effects);
                    }
                    targets.AddRange(team.Effects);
                    break;
            }
            return targets.Where(pe => (this as ILuaable).Valid(localteam, pe, s, v)).ToList();
        }

    }
}
