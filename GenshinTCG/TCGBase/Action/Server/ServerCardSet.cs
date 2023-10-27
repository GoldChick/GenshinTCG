using System.Text.Json;
using TCGBase;
using TCGRule;
using TCGUtil;

namespace TCGBase
{
    /// <summary>
    /// 通过客户端传递来的NetCardSet制作而成
    /// 使用前请确定是Valid的
    /// </summary>
    public class ServerPlayerCardSet : IPrintable
    {
        public bool Valid { get; init; }
        public AbstractCardCharacter[] CharacterCards { get; init; }
        public AbstractCardAction[] ActionCards { get; init; }
        public ServerPlayerCardSet(PlayerNetCardSet input)
        {
            try
            {
                CharacterCards = input.Characters.Select(s => Registry.Instance.CharacterCards[Normalize.NameIDNormalize(s)]).ToArray();
                ActionCards = input.ActionCards.Select(s => Registry.Instance.ActionCards[Normalize.NameIDNormalize(s)]).ToArray();
                Valid = CharacterCards.Length == 3
                    && ActionCards.Length == 30
                    && ActionCards.All(a => a.CanBeArmed());
            }
            catch (Exception e)
            {
                throw new ArgumentException($"ex:{e.Message}");
                //CharacterCards = Array.Empty<ICardCharacter>();
                //ActionCards = Array.Empty<ICardAction>();
                //Valid = false;
            }
        }

        public void Print()
        {
            Logger.Print("CharacterCards:");
            Logger.Print(JsonSerializer.Serialize(CharacterCards.Select(c => c.NameID)));
            Logger.Print("ActionCards:");
            Logger.Print(JsonSerializer.Serialize(ActionCards.Select(ac => ac.NameID)));
        }
    }
}
