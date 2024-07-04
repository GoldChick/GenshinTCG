namespace TCGBase
{
    public class LocalServer : AbstractServer
    {
        public LocalServer(AbstractClient localclient) : base()
        {
            PlayerClients.Add(localclient);
            State = ServerState.WaitingForOne;
        }
    }
}
