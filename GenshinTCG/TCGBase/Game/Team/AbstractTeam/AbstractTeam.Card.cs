namespace TCGBase
{
    public partial class AbstractTeam
    {
        public virtual int CardNum { get => 0; }
        public virtual void RollCard(int num, Type? type = null)
        {
        }
    }
}
