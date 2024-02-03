namespace TCGBase
{
    public interface ITalent
    {
        /// <summary>
        /// 不指定namespace，则和本身（这张卡）的一样
        /// </summary>
        public string CharacterNamespace { get; }
        public string CharacterNameID { get; }
        public bool IsFor(AbstractCardCharacter cha) => CharacterNameID == cha.NameID && CharacterNamespace == cha.Namespace;
    }
}
