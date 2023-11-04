namespace TCGBase
{
    public abstract class AbstractFood:AbstractCardEvent, ITargetSelector
    {
        /// <summary>
        /// 吃了之后会上什么buff
        /// </summary>
        public abstract AbstractCardPersistentEffect? AfterEatEffect { get; }
        public TargetEnum[] TargetEnums => new TargetEnum[] { TargetEnum.Character_Me };
    }
}
