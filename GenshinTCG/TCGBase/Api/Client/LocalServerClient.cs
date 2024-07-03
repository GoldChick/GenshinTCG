namespace TCGBase
{
    public class LocalServerClient : AbstractClient
    {
        public LocalServer Server { get; }
        public LocalServerClient() 
        {
            Server = new LocalServer(this);
            _server = Server;
        }
        public override ServerPlayerCardSet RequestCardSet()
        {
            throw new NotImplementedException();
        }

        public override NetEvent RequestEvent(OperationType demand)
        {
            throw new NotImplementedException();
        }
    }
}
