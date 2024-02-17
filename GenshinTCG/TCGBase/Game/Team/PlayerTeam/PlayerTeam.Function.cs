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
            Game.InnerHurt(damage, new(SenderTag.AfterHurt, TeamIndex, persistent, triggerable), specialAction);
        }
        public void DoHeal(HealRecord? heal, Persistent persistent, AbstractTriggerable triggerable)
        {
            Game.InnerHeal(heal, new(SenderTag.AfterHeal, TeamIndex, persistent, triggerable));
        }
        public void AttachElement(AbstractTriggerable source, DamageElement element, List<int> targetIndexs, bool targetRelative = true)
        {
            var absoluteIndexs = (targetRelative ? targetIndexs.Select(i => ((i + CurrCharacter) % Characters.Length + Characters.Length) % Characters.Length) : targetIndexs.Where(i => i >= 0 && i < Characters.Length));
            var evs = absoluteIndexs.Select(i => new ElementVariable(TeamIndex, element, DamageSource.Direct, i)).ToList();
            Action<PlayerTeam>? action = null;
            for (int i = 0; i < evs.Count; i++)
            {
                var ev = evs[i];
                evs.AddRange(Game.GetDamageReaction(ev));
                action += Game.ReactionActionGenerate(ev);
            }
            //TODO: reaction modifier 
            action?.Invoke(this);
        }
        /// <summary>
        /// 强制切换到某一个[活]角色（可指定绝对坐标或相对坐标，默认绝对）<br/>
        /// </summary>
        public void TrySwitchToIndex(int index, bool relative = false)
        {
            Debug.Assert(Characters.Any(c => c.Alive), "AbstractTeam.Prefab.SwitchToIndex():所有角色都已经死亡!");
            int curr = CurrCharacter;
            if (relative)
            {
                if (index % Characters.Length != 0)
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
                curr = (index + Characters.Length) % Characters.Length;
            }
            if (curr != CurrCharacter && Characters[curr].Alive)
            {
                var initial = CurrCharacter;
                CurrCharacter = curr;
                EffectTrigger(new AfterSwitchSender(TeamIndex, initial, CurrCharacter));
                SpecialState.DownStrike = true;
            }
        }
        /// <summary>
        /// 找到失去生命值最多的角色，默认值为当前出战<br/>
        /// except可以排除某个index的角色
        /// </summary>
        public int FindHPLostMost(int except = -1)
        {
            int currid = CurrCharacter;
            int currhplost = 0;
            for (int i = 0; i < Characters.Length; i++)
            {
                if (i != except)
                {
                    var c = Characters[i];
                    int hplost = c.CharacterCard.MaxHP - c.HP;
                    if (hplost > currhplost)
                    {
                        currid = i;
                        currhplost = hplost;
                    }
                }
            }
            return currid;
        }
    }
}
