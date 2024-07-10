namespace TCGBase
{
    public partial class Game : IGameAPI
    {
        public void Destory(Persistent p)
        {
            p.Active = false;
        }
    }
}
