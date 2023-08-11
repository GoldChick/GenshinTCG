namespace TCGBase
{
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

