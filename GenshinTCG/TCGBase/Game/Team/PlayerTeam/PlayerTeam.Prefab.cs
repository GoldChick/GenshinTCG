using System.Diagnostics;
namespace TCGBase
{
    //这里写出各种预设
    public partial class PlayerTeam
    {
        /// <summary>
        /// 强制切换到某一个角色（绝对坐标）
        /// </summary>
        public void SwitchToIndex(int index)
        {
            if (Characters[index].Alive)
            {
                Game.HandleEvent(new NetEvent(new NetAction(ActionType.SwitchForced, index)), TeamIndex);
            }
        }
        /// <summary>
        /// 强制切换下一个角色(没有则不切换)
        /// </summary>
        public void SwitchToNext()
        {
            Debug.Assert(Characters.Any(c => c.Alive), "AbstractTeam.Prefab.SwitchToNext():所有角色都已经死亡!");
            int curr = CurrCharacter;
            do
            {
                curr = (curr + 1) % Characters.Length;
            }
            while (!Characters[curr].Alive);
            Game.HandleEvent(new NetEvent(new NetAction(ActionType.SwitchForced, curr)), TeamIndex);
        }
        /// <summary>
        /// 强制切换上一个角色(没有则不切换)
        /// </summary>
        public void SwitchToLast()
        {
            Debug.Assert(Characters.Any(c => c.Alive), "AbstractTeam.Prefab.SwitchToLast():所有角色都已经死亡!");
            int curr = CurrCharacter;
            do
            {
                curr = (curr - 1) % Characters.Length;
            }
            while (!Characters[curr].Alive);
            Game.HandleEvent(new NetEvent(new NetAction(ActionType.SwitchForced, curr)), TeamIndex);
        }
        /// <summary>
        /// 找到失去生命值最多的角色，默认值为当前出战
        /// </summary>
        public int FindHPLostMost(int except = -1)
        {
            int currid = CurrCharacter;
            int currhplost = 0;
            for (int i = 0; i < Characters.Length; i++)
            {
                if (i!=except)
                {
                    var c = Characters[i];
                    int hplost = c.Card.MaxHP - c.HP;
                    if (hplost > currhplost)
                    {
                        currid = i;
                        currhplost=hplost;
                    }
                }
            }
            return currid;
        }
    }
}
