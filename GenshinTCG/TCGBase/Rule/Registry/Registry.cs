namespace TCGBase
{
    public class Registry
    {
        private readonly static Registry _instance = new();
        public static Registry Instance => _instance;
        public RegistryFromDll RFDll { get; }
        public RegistryFromJson RFJson { get; }
        internal RegistryCardCollection<AbstractCardCharacter> CharacterCards { get; }
        internal RegistryCardCollection<AbstractCardAction> ActionCards { get; }
        internal RegistryCardCollection<AbstractCardEffect> EffectCards { get; }
        internal RegistryCardCollection<AbstractCustomTriggerable> CustomTriggerable { get; }
        private Registry()
        {
            ActionCards = new();
            CharacterCards = new();
            EffectCards = new();

            CustomTriggerable = new();

            RFDll = new();
            RFJson = new();
        }
        public List<AbstractCardCharacter> GetCharacterCards() => CharacterCards.Select(kvp => kvp.Value).ToList();
        public List<AbstractCardAction> GetActionCards() => ActionCards.Select(kvp => kvp.Value).ToList();
        public List<AbstractCardEffect> GetEffectCards() => EffectCards.Select(kvp => kvp.Value).ToList();
    }
}
