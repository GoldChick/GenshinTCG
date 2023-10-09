using System.Text.Json;
using TCGBase;
using TCGCard;
using TCGUtil;

namespace TCGGame
{
    public class Character : IPrintable
    {
        public AbstractCardCharacter Card { get; protected set; }
        private int _hp;
        private int _mp;
        public AbstractPersistent<AbstractCardEffect>? Weapon { get; internal set; }
        public AbstractPersistent<AbstractCardEffect>? Artifact { get; internal set; }
        public AbstractPersistent<AbstractCardEffect>? Talent { get; internal set; }

        public PersistentSet<AbstractCardEffect> Effects { get; init; }

        public int HP
        {
            get { return _hp; }
            set { _hp = int.Clamp(value, 0, Card.MaxHP); }
        }
        public int MP { get => _mp; set => _mp = int.Clamp(value, 0, Card.MaxMP); }

        public bool Alive;
        /// <summary>
        /// 为active时可以使用技能
        /// </summary>
        public bool Active;
        public int Element;
        public Character(AbstractCardCharacter character)
        {
            Card = character;

            Effects = new();

            HP = Card.MaxHP;
            Alive = true;
            Active = true;
        }
        ///<returns>没用</returns>
        public void EffectTrigger(AbstractGame game, int meIndex, AbstractSender sender, AbstractVariable? variable)
        {
            Weapon?.EffectTrigger(game, meIndex, sender, variable);

            Artifact?.EffectTrigger(game, meIndex, sender, variable);

            Talent?.EffectTrigger(game, meIndex, sender, variable);

            Effects.EffectTrigger(game, meIndex, sender, variable);
        }

        public void Print()
        {
            //TODO: card nameid not clear
            Logger.Print($"{Card.NameID} {JsonSerializer.Serialize(Card.Tags)}", ConsoleColor.Cyan);
            Logger.Print($"ALIVE:{Alive}");
            if (Alive)
            {
                Logger.Print($"HP:{HP}/{Card.MaxHP} MP:{MP}/{Card.MaxMP} Element:{Element}");
            }
            Effects.Print();
        }
    }

}
