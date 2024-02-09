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

        public ActionRecordEffect(List<string>? add = null, List<string>? remove = null, DamageTargetTeam team = DamageTargetTeam.Me, CharacterTargetRecord? target = null) : base(TriggerType.Effect, team, target)
        {
            Add = add ?? new();
            Remove = remove ?? new();
        }
        public override EventPersistentHandler? GetHandler(ITriggerable triggerable)
        {
            return (me, p, s, v) =>
            {
                var team = Team == DamageTargetTeam.Enemy ? me.Enemy : me;
                var chars = Target.GetCharacters(team);

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
                            chars.ForEach(c => team.AddEffect(card, c));
                            break;
                    }
                }
            };
        }
    }
}
