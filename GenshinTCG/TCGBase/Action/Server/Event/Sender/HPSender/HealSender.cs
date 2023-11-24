namespace TCGBase
{
    /// <summary>
    /// 受到伤害后产生的sender，可能并不一定用于直接触发<br/>
    /// </summary>
    public class HealSender : AbstractSender
    {
        public override string SenderName => SenderTag.AfterHeal.ToString();
        public int Amount { get; internal set; }//set只是为了合并
        public int TargetIndex { get; init; }
        internal HealSender(int teamID, int amount,int targetindex) : base(teamID)
        {
            Amount = amount;
            TargetIndex = targetindex;
        }
    }
}
