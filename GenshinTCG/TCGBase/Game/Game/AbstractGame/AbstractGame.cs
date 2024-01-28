namespace TCGBase
{
    public abstract class AbstractGame
    {
        internal bool InstantTrigger { get; set; }
        internal Queue<Action> DelayedTriggerQueue { get; set; }
        public abstract int CurrTeam { get; protected set; }

        protected private AbstractGame()
        {
            DelayedTriggerQueue = new();
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
