namespace TCGBase
{
    /// <summary>
    /// 用于给EffectAct传递actioninfo的sender,
    /// 能被外界读取的值(除了Tags)必须<b>不可更改</b>
    /// </summary>
    public abstract class AbstractSender
    {
        /// <summary>
        /// 带有namespace的senderName,如"minecraft:switch"
        /// </summary>
        public abstract string SenderName { get; }
        /// <summary>
        /// 除了名字以外的动态可添加信息(如[重击]、[下落攻击]),
        /// 可以动态添加
        /// </summary>
        /// <remarks>TODO:将来实现mod可添加</remarks>
        public List<string> DynamicTags { get; } = new();
    }
}
