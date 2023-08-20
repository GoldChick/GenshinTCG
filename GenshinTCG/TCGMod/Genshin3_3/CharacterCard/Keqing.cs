using TCGBase;
using TCGCard;

namespace Genshin3_3
{
    public class Keqing : ICardCharacter
    {
        public string MainElement => TCGBase.Tags.ElementTags.ELECTRO;

        public int MaxHP => 10;

        public int MaxMP => 3;

        public IEffect? DefaultEffect => null;

        public ICardSkill[] Skills => new ICardSkill[] { };

        public string NameID => "keqing";

        public string[] Tags => throw new NotImplementedException();
    }
}
