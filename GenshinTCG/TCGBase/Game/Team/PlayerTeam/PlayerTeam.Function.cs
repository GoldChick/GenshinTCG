using System.Diagnostics;
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
            Game.InnerHurt(damage, new(TeamID, persistent, triggerable), specialAction);
        }
        public void AttachElement(Persistent persistent, AbstractTriggerable triggerable, DamageElement element, List<int> targetIndexs, bool targetRelative = true)
        {
            HurtSourceSender sourceSender = new(TeamID, persistent, triggerable);
            var absoluteIndexs = (targetRelative ? targetIndexs.Select(i => ((i + CurrCharacter) % Characters.Count + Characters.Count) % Characters.Count) : targetIndexs.Where(i => i >= 0 && i < Characters.Count));
            var evs = absoluteIndexs.Where(i => Characters[i].Alive).Select(i => new ElementVariable(TeamID, element, DamageSource.Direct, i)).ToList();
            Action? action = null;
            for (int i = 0; i < evs.Count; i++)
            {
                var ev = evs[i];
                evs.AddRange(Game.GetDamageReaction(ev));
                action += Game.ReactionActionGenerate(ev);

                Game.EffectTrigger(SenderTag.AfterElement, sourceSender, ev);
            }
            //TODO: reaction modifier 
            action?.Invoke();
        }

    }
}
