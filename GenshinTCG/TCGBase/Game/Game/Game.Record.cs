namespace TCGBase
{
    public partial class Game
    {
        /// <summary>
        /// 用来存储[队伍做出的]行动
        /// </summary>
        public List<List<BaseRecord>> NetEventRecords { get; }
        /// <summary>
        /// @desperated<br/>
        /// 用来存储[客观发生的]行动
        /// </summary>
        public List<List<BaseRecord>> ActionRecords { get; }
    }
}
