using System.Diagnostics;
using TCGBase;
namespace TCGGame
{
    //这里写出各种预设

    public abstract partial class AbstractTeam
    {
        /// <summary>
        /// 强制切换下一个角色(没有则不切换)<br/>
        /// NOTE:想要进行正确的[风压剑](或类似)结算，必须调用这个方法作为action参数！<br/>
        /// TODO:以后的版本可能会考虑修改？
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
    }
}
