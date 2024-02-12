namespace TCGBase
{
    public class DieSender : AbstractSender
    {
        public override string SenderName => SenderTag.PreDie.ToString();
        public int Cha_Index { get; }
        internal DieSender(int teamID, int cha_index) : base(teamID)
        {
            Cha_Index = cha_index;
        }
    }
}
