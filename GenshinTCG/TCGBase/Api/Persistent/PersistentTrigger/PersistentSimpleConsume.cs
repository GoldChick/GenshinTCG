namespace TCGBase
{
    /// <summary>
    /// 可用次数--
    /// </summary>
    public class PersistentSimpleConsume : PersistentTrigger
    {
        private Func<PlayerTeam, AbstractPersistent, AbstractSender, AbstractVariable?, bool>? _condition;
        public PersistentSimpleConsume(Func<PlayerTeam, AbstractPersistent, AbstractSender, AbstractVariable?, bool>? condition = null)
        {
            _condition = condition;
        }
        public override void Trigger(PlayerTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
        {
            if (_condition == null || _condition.Invoke(me, persitent, sender, variable))
            {
                persitent.AvailableTimes--;
            }
        }
    }
}
