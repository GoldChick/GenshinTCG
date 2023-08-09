namespace TCGAI
{
    /// <summary>
    /// 用来实现和AI的链接
    /// </summary>
    internal class AIConnector
    {
        internal enum ConnectType
        {
            Built_In,
            Socket
        }
        private readonly ConnectType _type;
        private readonly AbstractAI _ai;
        public AIConnector(ConnectType type)
        {
            _type = type;
        }
        public AIConnector(AbstractAI ai)
        {
            _type = ConnectType.Built_In;
            _ai= ai;
        }
        public AIEvent AskForEvent(AIEventType demand)
        {
            switch (_type)
            {
                case ConnectType.Built_In:
                    return BuiltInEvent(demand);
                case ConnectType.Socket:
                    break;
            }
            throw new Exception("AIConnector:AskForEvent()出现错误!");
        }
        private AIEvent BuiltInEvent(AIEventType demand)
        {
            return null;
        }
        private AIEvent SocketEvent() 
        {
            return null;
        }
    }
}
