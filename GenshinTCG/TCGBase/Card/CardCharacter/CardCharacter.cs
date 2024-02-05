namespace TCGBase
{
    /// <summary>
    /// 通过json创建的
    /// </summary>
    internal class CardCharacter : AbstractCardCharacter
    {
        public override int MaxHP { get; }
        public override int MaxMP { get; }
        public CardCharacter(CardRecordCharacter record) : base(record)
        {
            MaxHP = record.MaxHP;
            MaxMP = record.MaxMP;
        }
    }
}
