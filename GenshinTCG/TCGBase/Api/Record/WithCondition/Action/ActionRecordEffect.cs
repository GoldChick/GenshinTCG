using System.Xml.Linq;

namespace TCGBase
{
    /// <summary>
    /// 添加/删除summon/effect
    /// </summary>
    public record class ActionRecordEffect : ActionRecordBaseWithTarget
    {
        public List<string> Add { get; }
        public List<string> Remove { get; }

        public ActionRecordEffect(List<string>? add = null, List<string>? remove = null, TargetRecord? target = null, List<ConditionRecordBase>? when = null) : base(TriggerType.Effect, target, when)
        {
            Add = add ?? new();
            Remove = remove ?? new();
        }
        protected override void DoAction(AbstractTriggerable triggerable, PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v)
        {
            var chars = Target.GetTargets(me, p, s, v, out var team);

            foreach (var name in Remove)
            {
                if (Registry.Instance.EffectCards.TryGetValue(name, out var card))
                {
                    switch (card.CardType)
                    {
                        case CardType.Summon:
                            team.Summons.DestroyFirst(s => s.CardBase.Namespace == card.Namespace && s.CardBase.NameID == card.NameID);
                            break;
                        case CardType.Effect:
                            if (Target.Type == TargetType.Team)
                            {
                                team.Effects.DestroyFirst(s => s.CardBase.Namespace == card.Namespace && s.CardBase.NameID == card.NameID);
                            }
                            else
                            {
                                chars.ForEach(c =>
                                {
                                    if (c is Character cha)
                                    {
                                        cha.Effects.DestroyFirst(s => s.CardBase.Namespace == card.Namespace && s.CardBase.NameID == card.NameID);
                                    }
                                });
                            }
                            break;
                    }
                }
            }
            foreach (var name in Add)
            {
                if (Registry.Instance.EffectCards.TryGetValue(name, out var card))
                {
                    switch (card.CardType)
                    {
                        case CardType.Summon:
                            team.AddSummon(card);
                            break;
                        case CardType.Effect:
                            if (Target.Type == TargetType.Team)
                            {
                                team.AddEffect(card);
                            }
                            else
                            {
                                chars.ForEach(c => team.AddEffect(card, c.PersistentRegion));
                            }
                            break;
                    }
                }
            }
        }
    }
}
