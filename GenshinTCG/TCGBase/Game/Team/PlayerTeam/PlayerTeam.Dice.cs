namespace TCGBase
{
    public partial class PlayerTeam
    {
        public override int DiceNum => Dices.Count;
        public void RollDice(DiceRollingVariable rolling)
        {
            //TODO:是对的吗
            int num = int.Max(0, rolling.DiceNum - rolling.InitialDices.Count);
            List<int> randoms = new();

            for (int i = 0; i < num; i++)
                randoms.Add(Random.Next(8));

            AddDiceRange(randoms);
        }
        public void ReRollDice(DiceRollingVariable rolling)
        {
            List<int> randoms = new();
            randoms.AddRange(rolling.InitialDices);

            for (int i = 0; i < rolling.DiceNum; i++)
                randoms.Add(Random.Next(8));

            AddDiceRange(randoms);
        }
        public override int[] GetDicesArray()
        {
            int[] dices = new int[8];
            foreach (var d in Dices)
            {
                dices[d]++;
            }
            return dices;
        }
        public override List<(int count, int element)> GetSortedDices()
        {
            var dices = GetDicesArray().Select((d, element) => (d, element)).ToList();
            //by the way, 这高能==的优先级怎么是这样的
            dices.Sort((d1, d2) => d2.d - d1.d + (d2.element == 0 ? -100 : 0));
            return dices;
        }
        /// <summary>
        /// 获得某一种骰子的数量
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public int GetDiceNum(int type = 0) => Dices.FindAll(p => p == type).Count;
        /// <summary>
        /// 获得一个骰子,当骰子数量不满16个时才能获得成功。<br/>
        /// 会进行broadcast
        /// </summary>
        /// <returns>是否获得成功</returns>
        internal void AddSingleDice(int d)
        {
            if (Dices.Count < 16)
            {
                Dices.Add(int.Clamp(d, 0, 7));
                Dices.Sort();
                RealGame.BroadCast(ClientUpdateCreate.DiceUpdate(TeamIndex, Dices.ToArray()));
            }
        }
        public override void GainDice(params int[] dices)
        {
            foreach (var d in dices)
            {
                if (Dices.Count >= 16)
                {
                    break;
                }
                Dices.Add(int.Clamp(d, 0, 7));
            }
            Dices.Sort();
            RealGame.BroadCast(ClientUpdateCreate.DiceUpdate(TeamIndex, Dices.ToArray()));
        }

        /// <summary>
        /// 获得很多骰子<br/>
        /// 会进行broadcast
        /// </summary>
        public void AddDiceRange(IEnumerable<int> ds)
        {
            for (int i = 0; i < ds.Count(); i++)
            {
                if (Dices.Count >= 16)
                {
                    break;
                }
                Dices.Add(int.Clamp(ds.ElementAt(i), 0, 7));
            }
            Dices.Sort();
            RealGame.BroadCast(ClientUpdateCreate.DiceUpdate(TeamIndex, Dices.ToArray()));
        }
        public override void TryRemoveDice(int element)
        {
            if (Dices.Contains(element))
            {
                Dices.Remove(element);
            }
        }
        /// <summary>
        /// 数组每一位表示该元素消耗的数量
        /// </summary>
        internal void CostDices(params int[]? costs)
        {
            for (int i = 0; i < costs?.Length; i++)
            {
                for (int j = 0; j < costs[i]; j++)
                {
                    Dices.Remove(i);
                }
            }
            RealGame.BroadCast(ClientUpdateCreate.DiceUpdate(TeamIndex, Dices.ToArray()));
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
        public bool ContainsCost(int[]? supplier) => supplier == null || supplier.Select((value, index) => new { value, index }).All(p => p.value <= GetDiceNum(p.index));
    }

}
