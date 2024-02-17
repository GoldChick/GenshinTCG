namespace TCGBase
{
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
