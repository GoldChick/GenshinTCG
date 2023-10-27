﻿namespace TCGBase
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
                SenderName = SenderTag.PreDie.ToString();
            }else
            {
                SenderName = SenderTag.Die.ToString();
            }
            Cha_Index= cha_index;
        }
    }
}
