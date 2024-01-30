namespace TCGBase
{
    public abstract class AbstractGame
    {
        /// <summary>
        /// 不为null时，会存储非立即结算状态
        /// </summary>
        internal Queue<Action>? TempDelayedTriggerQueue { get; set; }
        public abstract int CurrTeam { get; protected set; }
        /// <summary>
        /// 用来存储[队伍做出的]行动
        /// </summary>
        public List<List<BaseRecord>> NetEventRecords { get; }
        /// <summary>
        /// @desperated<br/>
        /// 用来存储[客观发生的]行动
        /// </summary>
        public List<List<BaseRecord>> ActionRecords { get; }
        protected private AbstractGame()
        {
            NetEventRecords = new();
            ActionRecords = new();
        }
        public virtual void BroadCast(ClientUpdatePacket packet)
        {

        }
        /// <summary>
        /// TODO：偷个懒
        /// </summary>
        public virtual void BroadCastRegion()
        {

        }
        public virtual void EffectTrigger(AbstractSender sender, AbstractVariable? variable = null, bool broadcast = true)
        {

        }
        /// <summary>
        /// 这里处理所有被动的行为，并且默认行为可行（不是行动！）
        /// </summary>
        public virtual void TryProcessEvent(NetEvent evt, int teamid)
        {

        }
    }
}
