namespace TCGBase
{
    public partial class Game
    {
        internal static ReactionTags GetReaction(int currElement, DamageElement elementToApply, out int nextElementState)
        {
            ReactionTags reactiontag = ReactionTags.None;
            //角色身上附着的元素(只允许附着 无0 冰1 水2 火3 雷4 草6 <b>冰+草5</b>
            nextElementState = currElement;

            if (elementToApply is not (DamageElement.Trivial or DamageElement.Pierce))
            {
                nextElementState = 0;

                int reactionType = (int)elementToApply * 10 + currElement;
                switch (reactionType)
                {
                    case 12 or 21 or 25://冻结
                        reactiontag = ReactionTags.Frozen;
                        break;

                    case 13 or 31 or 35://融化
                        reactiontag = ReactionTags.Melt;
                        break;

                    case 14 or 41 or 45://超导
                        reactiontag = ReactionTags.SuperConduct;
                        break;

                    case 23 or 32://蒸发
                        reactiontag = ReactionTags.Vaporize;
                        break;

                    case 24 or 42://感电
                        reactiontag = ReactionTags.ElectroCharged;
                        break;

                    case 34 or 43://超载
                        reactiontag = ReactionTags.Overloaded;
                        break;

                    case 51 or 55://结晶
                        reactiontag = ReactionTags.CrystallizeCryo;
                        break;
                    case 52:
                        reactiontag = ReactionTags.CrystallizeHydro;
                        break;
                    case 53:
                        reactiontag = ReactionTags.CrystallizePyro;
                        break;
                    case 54:
                        reactiontag = ReactionTags.CrystallizeElectro;
                        break;

                    //NOTE:冰草共存优先反应冰

                    case 62 or 26://绽放
                        reactiontag = ReactionTags.Bloom;
                        break;

                    case 63 or 36://燃烧
                        reactiontag = ReactionTags.Burning;
                        break;

                    case 64 or 46://激化
                        reactiontag = ReactionTags.Catalyze;
                        break;

                    case 71 or 75://扩散
                        reactiontag = ReactionTags.SwirlCryo;
                        break;
                    case 72:
                        reactiontag = ReactionTags.SwirlHydro;
                        break;
                    case 73:
                        reactiontag = ReactionTags.SwirlPyro;
                        break;
                    case 74:
                        reactiontag = ReactionTags.SwirlElectro;
                        break;

                    case 61 or 16://不反应，但是冰草共存
                        nextElementState = 5;
                        break;

                    case 10 or 20 or 30 or 40 or 60://不反应，但是改变附着
                        nextElementState = (int)elementToApply;
                        break;

                    default://不反应，也不改变附着
                        nextElementState = currElement;
                        break;
                }

                //冰草共存检测是否反应掉了冰
                if (currElement == 5 && reactiontag != ReactionTags.None)
                {
                    nextElementState = 6;
                }
            }

            return reactiontag;
        }
        internal Action? ReactionActionGenerate(ElementVariable ev)
        {
            return ev.Reaction switch
            {
                ReactionTags.Overloaded => () =>
                {
                    var team = Teams[ev.TargetTeam];
                    if (ev.TargetIndex == team.CurrCharacter)
                    {
                        team.SwitchTo(1, true);
                    }
                }
                ,
                ReactionTags.Frozen => () =>
                {
                    if (Registry.Instance.EffectCards.TryGetValue("minecraft:effect_frozen", out var v))
                    {
                        Teams[ev.TargetTeam].AddEffect(v, ev.TargetIndex);
                    }
                }
                ,
                ReactionTags.Bloom => () =>
                {
                    if (Registry.Instance.EffectCards.TryGetValue("minecraft:effect_dendrocore", out var v))
                    {
                        Teams[1 - ev.TargetTeam].AddEffect(v, -1);
                    }
                }
                ,
                ReactionTags.Burning => () =>
                {
                    if (Registry.Instance.EffectCards.TryGetValue("minecraft:summon_burning", out var v))
                    {
                        Teams[1 - ev.TargetTeam].AddSummon(v);
                    }
                }
                ,
                ReactionTags.Catalyze => () =>
                {
                    if (Registry.Instance.EffectCards.TryGetValue("minecraft:effect_catalyzefield", out var v))
                    {
                        Teams[1 - ev.TargetTeam].AddEffect(v);
                    }
                }
                ,
                ReactionTags.CrystallizeCryo or ReactionTags.CrystallizeHydro or ReactionTags.CrystallizePyro or ReactionTags.CrystallizeElectro => () =>
                {
                    if (Registry.Instance.EffectCards.TryGetValue("minecraft:effect_crystal", out var v))
                    {
                        Teams[1 - ev.TargetTeam].AddEffect(v);
                    }
                }
                ,
                _ => null
            };
        }
        /// <summary>
        /// 在这里为DamageVariable设置Reaction，并为Character设置Element
        /// </summary>
        internal IEnumerable<DamageVariable> GetDamageReaction(ElementVariable evToPerson)
        {
            var currteam = Teams[evToPerson.TargetTeam];
            var cha = currteam.Characters[evToPerson.TargetIndex];

            ReactionTags tag = GetReaction(cha.Element, evToPerson.Element, out int nextElement);
            evToPerson.Amount += tag switch
            {
                ReactionTags.Vaporize or ReactionTags.Melt or ReactionTags.Overloaded => 2,
                ReactionTags.Frozen or ReactionTags.SuperConduct or ReactionTags.ElectroCharged or
                ReactionTags.Bloom or ReactionTags.Burning or ReactionTags.Catalyze or
                ReactionTags.CrystallizeCryo or ReactionTags.CrystallizeHydro or ReactionTags.CrystallizePyro or ReactionTags.CrystallizeElectro => 1,
                _ => 0
            };
            //TODO: anti element
            cha.Element = nextElement;
            evToPerson.Reaction = tag;

            var dvs = Enumerable.Range(0, currteam.Characters.Count)
                    .OrderBy(i => ((i - evToPerson.TargetIndex) % currteam.Characters.Count + currteam.Characters.Count) % currteam.Characters.Count)
                    .Skip(1).Where(i => currteam.Characters[i].Alive);

            return tag switch
            {
                ReactionTags.SuperConduct or ReactionTags.ElectroCharged => dvs.Select(i => new DamageVariable(currteam.TeamID, DamageElement.Pierce, 1, DamageSource.Indirect, i)),
                ReactionTags.SwirlCryo => dvs.Select(i => new DamageVariable(currteam.TeamID, DamageElement.Cryo, 1, DamageSource.Indirect, i)),
                ReactionTags.SwirlHydro => dvs.Select(i => new DamageVariable(currteam.TeamID, DamageElement.Hydro, 1, DamageSource.Indirect, i)),
                ReactionTags.SwirlPyro => dvs.Select(i => new DamageVariable(currteam.TeamID, DamageElement.Pyro, 1, DamageSource.Indirect, i)),
                ReactionTags.SwirlElectro => dvs.Select(i => new DamageVariable(currteam.TeamID, DamageElement.Electro, 1, DamageSource.Indirect, i)),
                _ => new List<DamageVariable>()
            };
        }
    }
}
