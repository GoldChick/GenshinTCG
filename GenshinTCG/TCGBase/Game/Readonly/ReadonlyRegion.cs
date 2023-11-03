using System.Text.Json;
using System.Text.Json.Serialization;

namespace TCGBase
{
    public class ReadonlyRegion
    {
        public int CurrCharacter { get; set; }
        public List<ReadonlyCharacter> Characters { get; }
        public List<ReadonlyPersistent> Effects { get; }
        public List<ReadonlyPersistent> Summons { get; }
        public List<ReadonlyPersistent> Supports { get; }
        [JsonConstructor]
        public ReadonlyRegion(int currCharacter, List<ReadonlyCharacter> characters, List<ReadonlyPersistent> effects, List<ReadonlyPersistent> summons, List<ReadonlyPersistent> supports)
        {
            CurrCharacter = currCharacter;
            Characters = characters;
            Effects = effects;
            Summons = summons;
            Supports = supports;
        }
        public ReadonlyRegion() { }
        public ReadonlyRegion(PlayerTeam pt)
        {
            Characters = pt.Characters.Select(c => new ReadonlyCharacter(c)).ToList();
            Effects = pt.Effects.Copy().Select(e => new ReadonlyPersistent(e.Card.TextureNameSpace, e.Card.TextureNameID, e.Card.Info(e))).ToList();
            Summons = pt.Summons.Copy().Select(e => new ReadonlyPersistent(e.Card.TextureNameSpace, e.Card.TextureNameID, e.Card.Info(e))).ToList();
            Supports = pt.Supports.Copy().Select(e => new ReadonlyPersistent(e.Card.TextureNameSpace, e.Card.TextureNameID, e.Card.Info(e))).ToList();
            CurrCharacter = pt.CurrCharacter;
        }
    }
}
