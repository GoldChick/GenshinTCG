namespace TCGBase
{
    public static class VariableTags
    {
        public static readonly string DICE_COST = "minecraft:dice_cost";
        public static readonly string DICE_ROLLING = "minecraft:dice_rolling";

        public static readonly string DAMAGE = "minecraft:damage";
        public static readonly string CAN_ACTION = "minecraft:can_action";
        public static readonly string FAST_ACTION = "minecraft:fast_action";
        public static readonly string ELEMENT_GENERATE = "minecraft:element_generate";
    }
    /// <summary>
    /// 用于给EffectAct传递actioninfo的variable
    /// 不同于sender,variable中的数组的[值]可以被更改
    /// </summary>
    public abstract class AbstractVariable
    {
        /// <summary>
        /// 带有namespace的variableName,如"minecraft:dicecost"
        /// </summary>
        public abstract string VariableName { get; }
    }
}

