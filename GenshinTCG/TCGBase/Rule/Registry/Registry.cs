namespace TCGBase
{
    public class Registry
    {
        private readonly static Registry _instance = new();
        public static Registry Instance => _instance;
        internal RegistryCardCollection<AbstractCardCharacter> CharacterCards { get; }
        internal RegistryCardCollection<AbstractCardAction> ActionCards { get; }
        internal RegistryCardCollection<AbstractCustomTriggerable> CustomTriggerable { get; }
        private Registry()
        {
            CharacterCards = new();
            ActionCards = new();
            CustomTriggerable = new();
        }
        public List<AbstractCardCharacter> GetCharacterCards() => CharacterCards.Select(kvp => kvp.Value).ToList();
        public List<AbstractCardAction> GetActionCards() => ActionCards.Select(kvp => kvp.Value).ToList();
    }
}
