using System.Text.Json.Serialization;

namespace TCGBase
{
    public enum EffectFatherType
    {
        None,
        First,
        Self
    }
    /// <summary>
    /// 添加/删除summon/effect
    /// </summary>
    public record class ActionRecordEffect : ActionRecordBaseWithTarget
    {
        /// <summary>
        /// 指定待添加状态的父子关系<br/>
        /// 注意：只对[召唤物]、[出战状态]、[单个角色状态]有效
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EffectFatherType Father { get; }
        public List<string> Add { get; }
        public List<string> Remove { get; }
        public ActionRecordEffect(EffectFatherType father = EffectFatherType.None, List<string>? add = null, List<string>? remove = null,TargetSupplyRecord? target = null, List<ConditionRecordBase>? when = null) : base(TriggerType.Effect, target, when)
        {
            Father = father;
            Add = add ?? new();
            Remove = remove ?? new();
        }
        protected override void DoAction(AbstractTriggerable triggerable, PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v)
        {
        //    var chars = Target.GetTargets(me, p, s, v);

        //    foreach (var name in Remove)
        //    {
        //        if (Registry.Instance.EffectCards.TryGetValue(name, out var card))
        //        {
        //            switch (card.CardType)
        //            {
        //                case CardType.Summon:
        //                    team.Summons.DestroyFirst(s => s.CardBase.Namespace == card.Namespace && s.CardBase.NameID == card.NameID);
        //                    break;
        //                case CardType.Effect:
        //                    if (Target.Type == TargetType.Team)
        //                    {
        //                        team.Effects.DestroyFirst(s => s.CardBase.Namespace == card.Namespace && s.CardBase.NameID == card.NameID);
        //                    }
        //                    else
        //                    {
        //                        chars.ForEach(c =>
        //                        {
        //                            if (c is Character cha)
        //                            {
        //                                cha.Effects.DestroyFirst(s => s.CardBase.Namespace == card.Namespace && s.CardBase.NameID == card.NameID);
        //                            }
        //                        });
        //                    }
        //                    break;
        //            }
        //        }
        //        else if (Registry.Instance.ActionCards.TryGetValue(name, out var accard))
        //        {
        //            team.CardsInHand.TryDestroyAll(c => c.Namespace == accard.Namespace && c.NameID == accard.NameID);
        //        }
        //    }

        //    var addeffects = Add.Where(Registry.Instance.EffectCards.ContainsKey).Select(name => Registry.Instance.EffectCards[name]);

        //    Persistent? father = Father == EffectFatherType.Self ? p : null;
        //    for (int i = 0; i < addeffects.Count(); i++)
        //    {
        //        var card = addeffects.ElementAt(i);
        //        Persistent toadd = new(card, father);

        //        if (i == 0 && Father == EffectFatherType.First)
        //        {
        //            father = toadd;
        //        }

        //        switch (card.CardType)
        //        {
        //            case CardType.Summon:
        //                team.AddSummon(toadd);
        //                break;
        //            case CardType.Effect:
        //                if (Target.Type == TargetType.Team)
        //                {
        //                    team.AddEffect(toadd);
        //                }
        //                else if (chars.Count == 1)
        //                {
        //                    team.AddEffect(toadd, chars[0].PersistentRegion);
        //                }
        //                else
        //                {
        //                    chars.ForEach(c => team.AddEffect(card, c.PersistentRegion));
        //                }
        //                break;
        //        }
        //    }

        //    var addactions = Add.Where(Registry.Instance.ActionCards.ContainsKey).Select(name => Registry.Instance.ActionCards[name]);
        //    foreach (var action in addactions)
        //    {
        //        team.GainCard(action);
        //    }
        }
    }
}
