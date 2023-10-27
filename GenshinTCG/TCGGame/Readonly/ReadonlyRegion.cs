namespace TCGGame
{
    public class ReadonlyRegion
    {
        public List<ReadonlyCharacter> Characters { get; }
        public List<ReadonlyPersistent> Effects { get; }
        public List<ReadonlyPersistent> Summons { get; }
        public List<ReadonlyPersistent> Supports { get; }
        public ReadonlyRegion(PlayerTeam pt)
        {
            Characters = pt.Characters.Select(c => new ReadonlyCharacter(c)).ToList() ;
            Effects = pt.Effects.Copy().Select(e => new ReadonlyPersistent(e.NameID, e.Card.Info(e))).ToList();
            Summons = pt.Summons.Copy().Select(e => new ReadonlyPersistent(e.NameID, e.Card.Info(e))).ToList();
            Supports = pt.Supports.Copy().Select(e => new ReadonlyPersistent(e.NameID, e.Card.Info(e))).ToList();
        }
    }
}
