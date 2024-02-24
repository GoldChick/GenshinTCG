namespace TCGBase
{
    public class ActionUseCardSender : AbstractSender, IMulPersistentSupplier
    {
        public override string SenderName => SenderTagInner.UseCard.ToString();
        public int Card { get; }
        public IEnumerable<Persistent> Persistents { get; }
        public ActionUseCardSender(int teamID, int card, IEnumerable<Persistent> persistents) : base(teamID)
        {
            Card = card;
            Persistents = persistents;
        }
    }
    /// <summary>
    /// 预期中只会用于json处理简单的多select问题
    /// </summary>
    internal class ActionDuringUseCardSender : AbstractSender, IMulPersistentSupplier
    {
        public override string SenderName => SenderTagInner.DuringUseCard.ToString();

        public IEnumerable<Persistent> Persistents { get; }

        public ActionDuringUseCardSender(int teamID, IEnumerable<Persistent> persistents) : base(teamID)
        {
            Persistents = persistents;
        }
    }
}
