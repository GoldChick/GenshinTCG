using System.Diagnostics;
using TCGBase;
namespace TCGGame
{
    //这里写出各种预设
    public partial class PlayerTeam
    {
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
    }
}
