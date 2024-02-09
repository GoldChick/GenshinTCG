namespace TCGBase
{
    /// <summary>
    /// 通过json创建的
    /// </summary>
    internal class CardCharacter : AbstractCardCharacter
    {
        public override string Namespace { get; }
        public override int MaxHP { get; }
        public override int MaxMP { get; }
        public CardCharacter(CardRecordCharacter record, string @namespace) : base(record)
        {
            MaxHP = record.MaxHP;
            MaxMP = record.MaxMP;
            Namespace = @namespace;
        }
    }
}
