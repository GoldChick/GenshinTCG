namespace TCGBase
{
    public class ActionCard
    {
        public AbstractCardAction Card { get; }
        /// <summary>
        /// 注册后的、带有namespace的nameid
        /// </summary>
        public string NameID { get; }

        public ActionCard(RegistryObject<AbstractCardAction> card)
        {
            NameID = card.NameID;
            Card = card.Value;
        }
    }
}
