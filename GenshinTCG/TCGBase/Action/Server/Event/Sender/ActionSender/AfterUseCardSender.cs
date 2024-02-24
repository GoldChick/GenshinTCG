
namespace TCGBase
{
    public class AfterUseCardSender : AbstractAfterActionSender, IPeristentSupplier, IMulPersistentSupplier
    {
        public override string SenderName => SenderTag.AfterUseCard.ToString();
        public Persistent Card { get; }
        public IEnumerable<Persistent> Persistents { get; }
        Persistent IPeristentSupplier.Persistent => Card;
        internal AfterUseCardSender(int teamID, Persistent card, IEnumerable<Persistent> persistents) : base(teamID)
        {
            Card = card;
            Persistents = persistents;
        }
    }
}
