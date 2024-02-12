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
        None = 0,

        Frozen = 12,
        Melt = 13,
        SuperConduct = 14,
        CrystallizeCryo = 15,
        SwirlCryo = 17,

        /// <summary>
        /// 蒸发
        /// </summary>
        Vaporize = 23,
        ElectroCharged = 24,
        CrystallizeHydro = 25,
        Bloom = 26,
        SwirlHydro = 27,

        Overloaded = 34,
        CrystallizePyro = 35,
        Burning = 36,
        SwirlPyro = 37,

        CrystallizeElectro = 45,
        /// <summary>
        /// 激化
        /// </summary>
        Catalyze = 46,
        SwirlElectro = 47,

        //↓desperated↓
        Crystallize = 55,
        /// <summary>
        /// 扩散
        /// </summary>
        Swirl = 77
    }
}
