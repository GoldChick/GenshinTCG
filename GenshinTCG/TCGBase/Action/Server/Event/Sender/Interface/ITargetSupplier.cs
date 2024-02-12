namespace TCGBase
{
    internal interface ITargetSupplier
    {
        public Persistent GetTarget(PlayerTeam team,int index);
    }
}
