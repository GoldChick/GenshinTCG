namespace TCGBase
{
    /// <summary>
    /// 用于给EffectAct传递actioninfo的sender
    /// 能被外界读取的值必须<b>不可更改</b>
    /// </summary>
    public abstract class AbstractSender
    {
        /// <summary>
        /// 带有namespace的senderName,如"minecraft:switch"
        /// </summary>
        public abstract string SenderName { get; }
    }
}
