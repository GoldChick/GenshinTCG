//专为我们lua设计
namespace TCGBase
{
    public partial class PlayerTeam
    {
        public Persistent? AddSummon(string nameid, Persistent? father = null)
        {
            if (Registry.Instance.EffectCards.TryGetValue(nameid, out var card) && card.CardType == CardType.Summon)
            {
                Summons.Add(new(card, father));
            }
            return null;
        }
        /// <summary>
        /// 将场上的状态p(卡牌)回手
        /// </summary>
        public void PopTo(Persistent p, TargetType type = TargetType.Hand)
        {
            p.Owner?.PopTo(p, CardsInHand);
        }
        /// <summary>
        /// 如果revive=false，则在目标角色被击倒时，会复苏
        /// </summary>
        public void Heal(Persistent persistent, AbstractTriggerable triggerable, int amount, int targetIndex = 0, bool targetRelative = true, bool revive = false)
        {
            var absoluteIndex = targetRelative ? ((targetIndex + CurrCharacter) % Characters.Length + Characters.Length) % Characters.Length : int.Clamp(targetIndex, 0, Characters.Length - 1);
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
    }
}
