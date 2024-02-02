namespace TCGBase
{
    public enum ElementCategory
    {
        /// <summary>
        /// 用于费用表示[同色骰]；用于元素表示无元素；
        /// </summary>
        Trival = 0,
        // ↓ 冰水火雷岩草风 ↓
        Cryo = 1,
        Hydro = 2,
        Pyro = 3,
        Electro = 4,
        Geo = 5,
        Dendro = 6,
        Anemo = 7,
        // ↓ 下面的仅仅用于费用↓
        Void = 8,
        MP = 9,
        Legend = 10,
    }

    public enum ReactionTags
    {
        None,
        Frozen,
        Melt,
        SuperConduct,
        /// <summary>
        /// 蒸发
        /// </summary>
        Vaporize,
        /// <summary>
        /// 感电
        /// </summary>
        ElectroCharged,
        Overloaded,
        Crystallize,
        Bloom,
        Burning,
        /// <summary>
        /// 激化
        /// </summary>
        Catalyze,
        /// <summary>
        /// 扩散
        /// </summary>
        Swirl
    }
}
