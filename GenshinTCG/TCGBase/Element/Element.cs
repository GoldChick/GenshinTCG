namespace TCGBase
{
    public enum ElementCategory
    {
        Trival = 0,
        Cryo = 1,
        Hydro = 2,
        Pyro = 3,
        Electro = 4,
        Geo = 5,
        Dendro = 6,
        Anemo = 7
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
