using TCGUtil;

namespace TCGBase
{
    public enum ElementCategory
    {
        TRIVAL = 0,
        CRYO = 1,
        HYDRO = 2,
        PYRO = 3,
        ELECTRO = 4,
        GEO = 5,
        DENDRO = 6,
        ANEMO = 7
    }
    public static class Element
    {
        public static int ElementStringToInt(string element) => element switch
        {
            "minecraft:cyro" => 1,
            "minecraft:hydro" => 2,
            "minecraft:pyro" => 3,
            "minecraft:electro" => 4,
            "minecraft:geo" => 5,
            "minecraft:dendro" => 6,
            "minecraft:anemo" => 7,
            _ => 0
        };
        public static string ElementIntToString(int element)
        {
            Logger.Error("Element.Element.ElementIntToString():输入的element超出0-7范围！已经自动作为Trival！", element < 0 || element > 7);
            return element switch
            {
                1 => Tags.ElementTags.CRYO,
                2 => Tags.ElementTags.HYDRO,
                3 => Tags.ElementTags.PYRO,
                4 => Tags.ElementTags.ELECTRO,
                5 => Tags.ElementTags.GEO,
                6 => Tags.ElementTags.DENDRO,
                7 => Tags.ElementTags.ANEMO,
                _ => Tags.ElementTags.TRIVAL
            };
        }
        public static string ElementEnumToString(ElementCategory element) => ElementIntToString((int)element);
    }
}
