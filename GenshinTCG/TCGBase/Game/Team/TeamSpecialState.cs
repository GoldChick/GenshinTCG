namespace TCGBase
{
    /// <summary>
    /// update see the last of <see cref="Game.EventProcess(NetEvent, int, out AbstractSender, out FastActionVariable?)"/>
    /// </summary>
    public class TeamSpecialState
    {
        public bool HeavyStrike { get; internal set; }
        public bool DownStrike { get; internal set; }
    }
}
