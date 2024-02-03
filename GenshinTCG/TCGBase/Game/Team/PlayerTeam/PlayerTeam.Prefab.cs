using System.Diagnostics;
namespace TCGBase
{
    //这里写出各种预设
    public partial class PlayerTeam
    {
        /// <summary>
        /// 尝试使用出战角色技能，自动clamp，注意：不是行动
        /// </summary>
        public void TryUseSkill(int skill)
        {
            //if (CurrCharacter >= 0)
            //{
            //    var c = Characters[CurrCharacter];
            //    if (c.Alive && c.Active && c.Card.Skills.Length > 0)
            //    {
            //        skill = int.Clamp(skill, 0, c.Card.Skills.Length);
            //        RealGame.TryProcessEvent(new NetEvent(new NetAction(ActionType.UseSKill, skill)), TeamIndex);
            //    }
            //}
        }
        public override void TrySwitchToIndex(int index, bool relative = false)
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
            if (Characters[curr].Alive)
            {
                RealGame.TryProcessEvent(new NetEvent(new NetAction(ActionType.SwitchForced, curr)), TeamIndex);
            }
        }
        public override int FindHPLostMost(int except = -1)
        {
            int currid = CurrCharacter;
            int currhplost = 0;
            for (int i = 0; i < Characters.Length; i++)
            {
                if (i != except)
                {
                    var c = Characters[i];
                    int hplost = c.Card.MaxHP - c.HP;
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
