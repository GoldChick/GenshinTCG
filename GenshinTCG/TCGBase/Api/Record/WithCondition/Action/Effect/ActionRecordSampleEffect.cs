//namespace TCGBase
//{
//    public record class ActionRecordSampleEffect : ActionRecordBaseWithTarget
//    {
//        public int Count { get; }
//        public int Max { get; }
//        public List<string> Pool { get; }
//        public ActionRecordSampleEffect(int count = 1, int max = -1, List<string>? pool = null, TargetSupplyRecord? target = null, List<ConditionRecordBase>? when = null) : base(TriggerType.Effect, target, when)
//        {
//            Pool = pool ?? new();
//            Max = max > 0 ? int.Min(max, Pool.Count) : Pool.Count;
//            Count = count;
//        }
//TODO:回顾一下如何sample
//        protected override void DoAction(AbstractTriggerable triggerable, PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v)
//        {
//            //var chars = Target.GetTargets(me, p, s, v).Where(pe => pe is Character c).ToList();

//            //var valids = Pool.Where(Registry.Instance.EffectCards.ContainsKey).Select(name => Registry.Instance.EffectCards[name]);

//            //var effects = valids.Where(ef => ef.CardType == CardType.Effect);
//            //var summons = valids.Where(ef => ef.CardType == CardType.Summon);

//            //var left_effects = effects.Where(ef => (Target.Type == TargetType.Team) ? !team.Effects.Contains(ef) : chars.Any(pe => pe is Character c && !c.Effects.Contains(ef)));
//            //var left_summons = summons.Where(ef => !team.Summons.Contains(ef));

//            //var left = left_effects.Concat(left_summons).ToList();

//            //var left_count = left_effects.Count() + int.Min(left_summons.Count(), team.Summons.MaxSize - team.Summons.Count) - (Pool.Count - Max);

//            //for (int i = 0; i < int.Min(left_count, Count); i++)
//            //{
//            //    int index = team.Random.Next(left.Count);
//            //    Add(team, chars, left[index]);
//            //    left.RemoveAt(index);
//            //    //sample left & add
//            //}
//            //for (int i = 0; i < int.Max(0, Count - left_count); i++)
//            //{
//            //    Add(team, chars, valids.ElementAt(team.Random.Next(valids.Count())));
//            //    //sample all & update
//            //}
//        }
//        private static void Add(PlayerTeam team, List<Persistent> chars, CardEffect ef)
//        {
//            Persistent pe = new(ef);
//            if (ef.CardType == CardType.Summon)
//            {
//                team.Summons.Add(pe);
//            }
//            else
//            {
//                chars.ForEach(c => team.AddEffect(pe, c.PersistentRegion));
//            }
//        }
//    }
//}
