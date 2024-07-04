namespace TCGBase
{
    public enum ElementCategory
    {
        /// <summary>
        /// 用于费用表示[同色骰]；用于元素表示无元素；
        /// </summary>
        Trivial = 0,
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
    public enum DamageElement
    {
        Trivial = 0,
        Cryo = 1,
        Hydro = 2,
        Pyro = 3,
        Electro = 4,
        Geo = 5,
        Dendro = 6,
        Anemo = 7,
        Pierce = 8,
    }
}
