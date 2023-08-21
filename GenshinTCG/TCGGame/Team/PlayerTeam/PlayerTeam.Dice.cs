﻿using TCGBase;
using TCGUtil;

namespace TCGGame
{
    public partial class PlayerTeam
    {
        public void Roll(DiceRollingVariable rolling)
        {
            Logger.Print($"{rolling.DiceNum}");
            int num = int.Max(0, rolling.DiceNum - rolling.InitialDices.Count);
            for (int i = 0; i < num; i++)
                AddDice(Random.Next(8));
            for (int i = 0; i < rolling.RollingTimes; i++)
                ReRoll();
        }
        private void ReRoll()
        {
            Logger.Error("PlayerTeam:ReRoll还没做");
            //TODO:need request
        }

        /// <summary>
        /// 返回int[8],第i个成员表示第i种元素骰的数量(默认顺序:万能 冰水火雷岩草风)
        /// </summary>
        public int[] GetDiceNum()
        {
            int[] nums = { 0, 0, 0, 0, 0, 0, 0, 0 };
            foreach (int i in Dices)
                if (i >= 0 && i <= 7)
                    nums[i]++;
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

    }

}