namespace TCGBase
{
    public partial class Game
    {
        public Dictionary<int, Persistent> GlobalPersistents { get; }
        /// <summary>
        /// 每次(给出)行动对应一个frame
        /// </summary>
        public int FrameID { get; private set; }
        /// <summary>
        /// 为状态分布的全局自增ID
        /// </summary>
        public int CurrPersistentID { get; private set; }
        internal void RegisterPersistent(Persistent p)
        {
            p.ID = CurrPersistentID;
            GlobalPersistents.Add(CurrPersistentID, p);
            CurrPersistentID += 1;
        }
        internal void UnregisterPersistent(Persistent p)
        {
            GlobalPersistents.Remove(p.ID);
        }
    }
}
