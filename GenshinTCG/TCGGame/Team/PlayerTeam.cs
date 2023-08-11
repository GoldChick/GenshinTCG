using System.Text.Json;
using TCGBase;

namespace TCGGame
{
    public class PlayerTeam:AbstractTeam
    {
        /// <summary>
        /// max_size=16,默认顺序为 万能 冰水火雷岩草风(0-7)
        /// </summary>
        protected List<int> Dices { get; } = new();

        /// <param name="cardset">经过处理确认正确的卡组</param>
        public PlayerTeam(PlayerCardSet cardset)
        {
            UseDice= true;

        }

        /// <summary>
        /// 获得int[8],第i个成员表示第i种元素骰的数量(默认顺序:万能 冰水火雷岩草风)
        /// </summary>
        /// <returns></returns>
        public int[] GetDiceNum()
        {
            int[] nums = { 0, 0, 0, 0, 0, 0, 0, 0 };
            foreach (int i in Dices)
            {
                if (i >= 0 && i <= 7)
                {
                    nums[i]++;
                }
            }
            return nums;
        }
        /// <summary>
        /// 获得一个骰子,当骰子数量不满16个时才能获得成功
        /// </summary>
        /// <returns>是否获得成功</returns>
        public bool AddDice(int d)
        {
            if (Dices.Count < 16)
            {
                d = int.Clamp(d, 0, 7);
                Dices.Add(d);
                //从小到大排序
                Dices.Sort();
                return true;
            }
            return false;
        }


        public ReadonlyTeam ToReadonly()
        {
            return null;
        }
    }

}
