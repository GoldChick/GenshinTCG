namespace TCGBase
{
    /// <summary>
    /// 结算增伤减伤所使用的sender ,包含根本source <see cref="IDamageSource"/>
    /// </summary>
    public class PreHurSender : AbstractSender
    {
        public override string SenderName { get; }
        public IDamageSource RootSource { get; init; }
        internal PreHurSender(int teamID, IDamageSource ds, SenderTag sender) : this(teamID,ds,sender.ToString()){}
        internal PreHurSender(int teamID,IDamageSource ds, string sender) : base(teamID)
        {
            RootSource = ds;
            SenderName = sender;
        }
    }
}
