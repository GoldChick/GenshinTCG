using System.Text.Json;
using TCGBase;
using TCGCard;
using TCGUtil;

namespace TCGGame
{
    public class Character : IPrintable
    {
        public ICardCharacter Card { get; protected set; }
        private int _hp;
        private int _mp;
        public AbstractPersistent<IEffect>? Weapon { get; internal set; }
        public AbstractPersistent<IEffect>? Artifact { get; internal set; }
        public AbstractPersistent<IEffect>? Talent { get; internal set; }

        public PersistentSet<IEffect> Effects { get; init; }

        public int HP
        {
            get { return _hp; }
            set { _hp = int.Clamp(value, 0, Card.MaxHP); }
        }
        public int MP { get => _mp; set => _mp = int.Clamp(value, 0, Card.MaxMP); }
        public bool Alive;
        public int Element;
        public Character(ICardCharacter character)
        {
            Card = character;

            Effects = new();

            HP = Card.MaxHP;
            HP = Card.MaxMP;
            Alive = true;
        }
        ///<returns>没用</returns>
        public bool EffectTrigger(AbstractGame game, int meIndex, AbstractSender sender, AbstractVariable? variable)
        {
            Weapon?.EffectTrigger(game, meIndex, sender, variable);

            Artifact?.EffectTrigger(game, meIndex, sender, variable);

            Talent?.EffectTrigger(game, meIndex, sender, variable);


            return true;
        }

        public void Print()
        {
            //TODO: card nameid not clear
            Logger.Print($"{Card.NameID} {JsonSerializer.Serialize(Card.Tags)}");
            Logger.Print($"ALIVE:{Alive}");
            if (Alive)
            {
                Logger.Print($"HP:{HP}/{Card.MaxHP} MP:{MP}/{Card.MaxMP} Element:{Element}");
            }
        }
    }

}
