namespace TCGBase
{
    /// <summary>
    /// talent并非基本类型，而是可选项
    /// </summary>
    public interface ICardTalent
    {
        /// <summary>
        /// 不指定namespace，则和本身（这张卡）的一样
        /// </summary>
        public string CharacterNamespace { get; }
        public string CharacterNameID { get; }
    }
}
