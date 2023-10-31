namespace TCGBase
{
    public class Enchant : AbstractCardPersistentEffect
    {
        public override string TextureNameID { get; }
        public override int MaxUseTimes { get; }
        public override PersistentTriggerDictionary TriggerDic { get; }
        /// <param name="round">为true是回合制effect，为false是次数effect</param>
        public Enchant(int element, int maxusetimes, bool round = true, int adddamage = 0)
        {
            TextureNameID = $"enchant_{((ElementCategory)element).ToString().ToLower()}";
            MaxUseTimes = maxusetimes;
            TriggerDic = new()
            {
                { SenderTag.ElementEnchant, new PersistentElementEnchant(element, !round,adddamage) }
            };
            if (round)
            {
                TriggerDic.Add(SenderTag.RoundOver, (me, p, s, v) => p.AvailableTimes--);
            }
        }
    }
}
