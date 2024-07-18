namespace TCGBase
{
    public class ActionUseCardSender : SimpleSender, IMulPersistentSupplier, ITriggerableIndexSupplier
    {
        public int Card { get; }
        public IEnumerable<Persistent> Persistents { get; }
        int ITriggerableIndexSupplier.SourceIndex => throw new NotImplementedException("ActionUseCardSender: 作为ITriggerableIndexSupplier，没有SourceIndex这个信息。（一般用来指定技能、准备技能的角色）");
        int ITriggerableIndexSupplier.TriggerableIndex => Card;

        public ActionUseCardSender(int teamID, int card, IEnumerable<Persistent> persistents) : base(teamID)
        {
            Card = card;
            Persistents = persistents;
        }
    }
    /// <summary>
    /// 预期中只会用于json处理简单的多select问题
    /// </summary>
    internal class ActionDuringUseCardSender : SimpleSender, IMulPersistentSupplier
    {
        //public override string SenderName => SenderTagInner.DuringUseCard.ToString();

        public IEnumerable<Persistent> Persistents { get; }

        public ActionDuringUseCardSender(int teamID, IEnumerable<Persistent> persistents) : base(teamID)
        {
            Persistents = persistents;
        }
    }
}
