namespace TCGBase
{
    public interface ICardFood
    {
        /// <summary>
        /// 吃了之后会上什么buff
        /// </summary>
        public abstract AbstractCardPersistent? AfterEatEffect { get; }

    }
}
