namespace TCGBase
{
    /// <summary>
    /// PRE_DIE和DIE共用
    /// </summary>
    internal class DieSender : AbstractSender
    {
        public override string SenderName {  get; }
        public int Cha_Index { get;  }
        public DieSender(int teamID,int cha_index,bool pre=false) : base(teamID)
        {
            if (pre)
            {
                SenderName = Tags.SenderTags.PRE_DIE;
            }else
            {
                SenderName = Tags.SenderTags.DIE;
            }
            Cha_Index= cha_index;
        }
    }
}
