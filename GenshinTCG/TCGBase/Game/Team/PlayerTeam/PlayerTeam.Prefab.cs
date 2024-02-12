using System.Diagnostics;
namespace TCGBase
{
    //这里写出各种预设
    public partial class PlayerTeam
    {
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
