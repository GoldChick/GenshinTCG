using System.Diagnostics;

namespace TCGBase
{
    public partial class PlayerTeam : IPlayerTeamAPI
    {
        public Persistent? AddCard(string nameid)
        {
            return Registry.Instance.ActionCards.TryGetValue(nameid, out var card) ? CardsInHand.Add(card) : null;
        }
        public Persistent? AddEffect(string nameid, Persistent? father = null, object? targetArea = null)
        {
            if (Registry.Instance.EffectCards.TryGetValue(nameid, out var card) && card.CardType == CardType.Effect)
            {
                if (Characters.ElementAtOrDefault((targetArea as Persistent)?.PersistentRegion ?? (targetArea as int?) ?? -1) is Character character)
                {
                    return character.AddEffect(card);
                }
                else
                {
                    Persistent p = new(card, father);
                    Effects.Add(p);
                    return p;
                }
            }
            return null;
        }
        public Persistent? AddSummon(string nameid, Persistent? father = null)
        {
            if (Registry.Instance.EffectCards.TryGetValue(nameid, out var card) && card.CardType == CardType.Summon)
            {
                Summons.Add(new(card, father));
            }
            return null;
        }
        public void PopTo(Persistent p, TargetType type = TargetType.Hand)
        {
            p.Owner?.PopTo(p, CardsInHand);
        }
        public void GainDice(object element, int count = 1)
        {
            int dice_element = element is ElementCategory || element is int ? (int)element : 0;

            for (int i = 0; i < count; i++)
            {
                if (Dices.Count >= 16)
                {
                    break;
                }
                Dices.Add(int.Clamp(dice_element, 0, 7));
                Dices.Sort();
                Game.BroadCast(ClientUpdateCreate.DiceUpdate(TeamID, Dices.ToArray()));
            }
        }
        public void Heal(Persistent persistent, AbstractTriggerable triggerable, int amount, int targetIndex = 0, bool targetRelative = true, bool revive = false)
        {
            var absoluteIndex = targetRelative ? ((targetIndex + CurrCharacter) % Characters.Count + Characters.Count) % Characters.Count : int.Clamp(targetIndex, 0, Characters.Count - 1);
            var hv = new HealVariable(TeamID, amount, DamageSource.Direct, absoluteIndex);

            var cha = Characters[hv.TargetIndex];
            if (!cha.Alive && revive)
            {
                cha.Revive();
            }
            if (cha.Alive)
            {
                hv.Amount = int.Min(hv.Amount, cha.Card.MaxHP - cha.HP);
                cha.HP += hv.Amount;
                Game.BroadCast(ClientUpdateCreate.CharacterUpdate.HealUpdate(hv.TargetTeam, hv.TargetIndex, hv.Amount));

                Game.EffectTrigger(new HurtSourceSender(SenderTag.AfterHeal, TeamID, persistent, triggerable), hv);
            }
        }
        public void UseSkill(Character c, int skill_index)
        {
            Game.EffectTrigger(new ActionUseSkillSender(TeamID, c.PersistentRegion, skill_index));
        }
        public void PrepareSkill(Character c, int prepareskill_index)
        {
            Game.EffectTrigger(new ActionUseSkillSender(TeamID, c.PersistentRegion, prepareskill_index));
        }
        public void Trigger(Persistent source, string senderID)
        {
            Game.EffectTrigger(new SimpleSourceSender(TeamID, senderID, source));
        }
        public void SwitchTo(int targetIndex, bool targetRelative = false)
        {
            targetIndex = (targetIndex % Characters.Count + Characters.Count) % Characters.Count;
            Debug.Assert(Characters.Any(c => c.Alive), "AbstractTeam.Prefab.SwitchToIndex():所有角色都已经死亡!");
            int curr = CurrCharacter;
            if (targetRelative)
            {
                if (targetIndex != 0)
                {
                    do
                    {
                        curr = (curr + targetIndex) % Characters.Count;
                    }
                    while (!Characters[curr].Alive);
                }
            }
            else
            {
                curr = targetIndex;
            }
            if (curr != CurrCharacter && Characters[curr].Alive)
            {
                var initial = CurrCharacter;
                CurrCharacter = curr;
                Game.EffectTrigger(new AfterSwitchSender(TeamID, initial, CurrCharacter));
                SpecialState.DownStrike = true;
            }
        }
        public void DrawCard(int num = 1, params string[] tags)
        {
            var valid_cards = LeftCards.Select((item, idx) => (item, idx)).Where(tuple => tags.All(tuple.item.Tags.Contains)).Take(num).ToList();

            valid_cards.Reverse();

            foreach (var (item, idx) in valid_cards)
            {
                LeftCards.RemoveAt(idx);
                CardsInHand.Add(item);
            }
        }
        //public void Discard()
        //{
        //}
    }
}
