using System.Text.Json;

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

            var removecards = Remove.Select(str => Registry.Instance.EffectCards[str]);
            foreach (var card in removecards)
            {
                switch (card.CardType)
                {
                    case CardType.Summon:
                        //TODO: remove summon
                        break;
                    case CardType.Effect:
                        //: remove effect
                        if (Target.Type == TargetType.Team)
                        {

                        }
                        else
                        {

                        }
                        break;
                }
            }
            var addcards = Add.Select(str => Registry.Instance.EffectCards[str]);
            foreach (var card in addcards)
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
