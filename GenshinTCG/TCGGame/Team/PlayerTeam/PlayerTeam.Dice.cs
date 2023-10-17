using TCGBase;
using TCGUtil;

namespace TCGGame
{
    public partial class PlayerTeam
    {
        public void RollDice(DiceRollingVariable rolling)
        {
            Logger.Print($"{rolling.DiceNum}");
            int num = int.Max(0, rolling.DiceNum - rolling.InitialDices.Count);
            for (int i = 0; i < num; i++)
                AddDice(Random.Next(8));
            for (int i = 0; i < rolling.RollingTimes; i++)
                ReRollDice();
        }
        private void ReRollDice()
        {
            Logger.Error("PlayerTeam:ReRoll还没做");
            //TODO:need request
        }

        /// <summary>
        /// 返回int[8],第i个成员表示第i种元素骰的数量(默认顺序:万能 冰水火雷岩草风)
        /// </summary>
        public int[] GetDices()
        {
            int[] nums = { 0, 0, 0, 0, 0, 0, 0, 0 };
            foreach (int i in Dices)
                if (i >= 0 && i <= 7)
                    nums[i]++;
            return nums;
        }
        /// <summary>
        /// 获得某一种骰子的数量
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override int GetDiceNum(int type = 0) => Dices.FindAll(p=>p==type).Count;
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
        public void CostDices(params int[]? costs)
        {
            for (int i = 0; i < costs?.Length ; i++)
            {
                for (int j = 0; j < costs[i]; j++)
                {
                    Dices.Remove(i);
                }
            }
        }
        /// <summary>
        /// 是否包含所需要的骰子
        /// 此时index=0为万能骰
        /// </summary>
        /// <param name="require">以list形式输入的</param>
        public bool ContainsDice(int[]? require) => require == null || require.GroupBy(n => int.Clamp(n, 0, 7)).All(g => g.Count() <= GetDiceNum(g.Key));

        /// <summary>
        /// 是否包含所需要的骰子
        /// 此时index=0为万能骰
        /// </summary>
        /// <param name="supplier">以int[8]形式输入的</param>
        public bool ContainsCost(int[]? supplier) => supplier == null || supplier.Select((value, index) => new { value, index }).All(p => p.value<=GetDiceNum(p.index));
    }

}
