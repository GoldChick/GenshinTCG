using Minecraft;

namespace TCGBase
{
    /*
     * 注册流程：
     * 首先，注册内部的triggerable、card
     * 然后，注册外部的triggerable
     * 最后，注册外部的card
     */
    public class Registry
    {
        private readonly static Registry _instance = new();
        public static Registry Instance => _instance;
        public RegistryFromDll RFDll { get; }
        public RegistryFromJson RFJson { get; }
        internal RegistryCardCollection<CardCharacter> CharacterCards { get; }
        internal RegistryCardCollection<AbstractCardAction> ActionCards { get; }
        internal RegistryCardCollection<CardEffect> EffectCards { get; }
        internal RegistryCardCollection<AbstractTriggerable> CustomTriggerable { get; }
        private Registry()
        {
            ActionCards = new();
            CharacterCards = new();
            EffectCards = new();

            CustomTriggerable = new();

            RFDll = new();
            RFJson = new();


            var util = new Minecraft_Util();
            util.GetRegister().RegisterTriggerable(CustomTriggerable);
        }

        public List<CardCharacter> GetCharacterCards() => CharacterCards.Select(kvp => kvp.Value).ToList();
        public List<AbstractCardAction> GetActionCards() => ActionCards.Select(kvp => kvp.Value).ToList();
        public List<CardEffect> GetEffectCards() => EffectCards.Select(kvp => kvp.Value).ToList();
        public List<AbstractTriggerable> GetTriggerables() => CustomTriggerable.Select(kvp => kvp.Value).ToList();
    }
}
