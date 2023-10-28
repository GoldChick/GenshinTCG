namespace TCGBase
{
    public abstract class AbstractNetCardSet
    {
    }
    public class PlayerNetCardSet : AbstractNetCardSet
    {
        /// <summary>
        /// size=3
        /// </summary>
        public string[] Characters { get; init; }
        /// <summary>
        /// size=30
        /// </summary>
        public string[] ActionCards { get; init; }
    }
}
