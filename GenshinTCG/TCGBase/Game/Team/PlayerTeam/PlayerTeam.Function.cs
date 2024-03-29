﻿using System.Diagnostics;
namespace TCGBase
{
    public partial class PlayerTeam
    {
        /// <summary>
        /// 根据DamageVariable.DamageTargetTeam,对<b>我方队伍</b>或<b>对方队伍</b>造成伤害<br/>
        /// 对我方队伍造成伤害时，不能吃到对方队伍的增伤
        /// </summary>
        public void DoDamage(DamageRecord? damage, Persistent persistent, AbstractTriggerable triggerable, Action? specialAction = null)
        {
            Game.InnerHurt(damage, new(SenderTag.AfterHurt, TeamIndex, persistent, triggerable), specialAction);
        }
        /// <summary>
        /// 如果revive=false，则在目标角色被击倒时，会复苏
        /// </summary>
        public void Heal(Persistent persistent, AbstractTriggerable triggerable, int amount, int targetIndex, bool targetRelative = true, bool revive = false)
        {
            var absoluteIndex = targetRelative ? ((targetIndex + CurrCharacter) % Characters.Length + Characters.Length) % Characters.Length : int.Clamp(targetIndex, 0, Characters.Length - 1);
            var hv = new HealVariable(TeamIndex, amount, DamageSource.Direct, absoluteIndex);

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

                Game.EffectTrigger(new HurtSourceSender(SenderTag.AfterHeal, TeamIndex, persistent, triggerable), hv);
            }
        }
        public void AttachElement(Persistent persistent, AbstractTriggerable triggerable, DamageElement element, List<int> targetIndexs, bool targetRelative = true)
        {
            HurtSourceSender sourceSender = new(SenderTag.AfterElement, TeamIndex, persistent, triggerable);
            var absoluteIndexs = (targetRelative ? targetIndexs.Select(i => ((i + CurrCharacter) % Characters.Length + Characters.Length) % Characters.Length) : targetIndexs.Where(i => i >= 0 && i < Characters.Length));
            var evs = absoluteIndexs.Where(i => Characters[i].Alive).Select(i => new ElementVariable(TeamIndex, element, DamageSource.Direct, i)).ToList();
            Action? action = null;
            for (int i = 0; i < evs.Count; i++)
            {
                var ev = evs[i];
                evs.AddRange(Game.GetDamageReaction(ev));
                action += Game.ReactionActionGenerate(ev);

                Game.EffectTrigger(sourceSender, ev);
            }
            //TODO: reaction modifier 
            action?.Invoke();
        }
        /// <summary>
        /// 强制切换到某一个[活]角色（可指定绝对坐标或相对坐标，默认绝对）<br/>
        /// </summary>
        public void TrySwitchToIndex(int index, bool relative = false)
        {
            index = (index % Characters.Length + Characters.Length) % Characters.Length;
            Debug.Assert(Characters.Any(c => c.Alive), "AbstractTeam.Prefab.SwitchToIndex():所有角色都已经死亡!");
            int curr = CurrCharacter;
            if (relative)
            {
                if (index != 0)
                {
                    do
                    {
                        curr = (curr + index) % Characters.Length;
                    }
                    while (!Characters[curr].Alive);
                }
            }
            else
            {
                curr = index;
            }
            if (curr != CurrCharacter && Characters[curr].Alive)
            {
                var initial = CurrCharacter;
                CurrCharacter = curr;
                Game.EffectTrigger(new AfterSwitchSender(TeamIndex, initial, CurrCharacter));
                SpecialState.DownStrike = true;
            }
        }

    }
}
