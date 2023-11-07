using TCGBase;

namespace TCGBase
{
    /// <summary>
    /// 通过客户端传递来的NetCardSet制作而成
    /// 使用前请确定是Valid的
    /// </summary>
    public class ServerPlayerCardSet
    {
        public bool Valid { get; init; }
        internal List<AbstractCardCharacter> CharacterCards { get; init; }
        internal List<AbstractCardAction> ActionCards { get; init; }
        public ServerPlayerCardSet(PlayerNetCardSet input)
        {
            try
            {
                CharacterCards = input.Characters.Select(s =>  Registry.Instance.CharacterCards[s]).ToList();
                ActionCards = input.ActionCards.Select(s => Registry.Instance.ActionCards[s]).ToList();
                Valid = CharacterCards.Count == 3
                    && ActionCards.Count == 30
                    && ActionCards.All(a => a.CanBeArmed());
            }
            catch (Exception e)
            {
                throw new ArgumentException($"ex:{e.Message}");
            }
        }
    }
}
