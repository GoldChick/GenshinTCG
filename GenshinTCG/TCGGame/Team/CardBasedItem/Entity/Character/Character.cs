using System.Text.Json;
using TCGBase;
using TCGUtil;

namespace TCGGame
{
    public class Character : IPrintable
    {
        public AbstractCardCharacter Card { get; protected set; }
        /// <summary>
        /// 表示自己在team中的位置
        /// </summary>
        public int Index { get; init; }
        private int _hp;
        private int _mp;
        public Persistent<AbstractCardPersistentEffect>? Weapon { get; internal set; }
        public Persistent<AbstractCardPersistentEffect>? Artifact { get; internal set; }
        public Persistent<AbstractCardPersistentTalent>? Talent { get; internal set; }

        public PersistentSet<AbstractCardPersistentEffect> Effects { get; init; }

        public int HP
        {
            get { return _hp; }
            set { _hp = int.Clamp(value, 0, Card.MaxHP); }
        }
        public int MP { get => _mp; set => _mp = int.Clamp(value, 0, Card.MaxMP); }

        public bool Alive;
        /// <summary>
        /// 濒死状态，生命值降为0，又不被“免于被击倒”治疗会使其为true<br/>
        /// 真死了之后又为false
        /// </summary>
        public bool Predie;
        /// <summary>
        /// 为active时可以使用技能
        /// </summary>
        public bool Active;
        public int Element;
        public Character(AbstractCardCharacter character,int index)
        {
            Card = character;
            Index=index; 

            Effects = new(index);

            HP = Card.MaxHP;
            Alive = true;
            Active = true;
        }
        ///<returns>没用</returns>
        public void EffectTrigger(PlayerTeam me, AbstractSender sender, AbstractVariable? variable)
        {
            Weapon?.EffectTrigger(me, sender, variable);

            Artifact?.EffectTrigger(me, sender, variable);

            Talent?.EffectTrigger(me, sender, variable);

            Effects.EffectTrigger(me, sender, variable);
        }

        public void Print()
        {
            //TODO: card nameid not clear
            Logger.Print($"{Card.NameID} {JsonSerializer.Serialize(Card.SpecialTags)}", ConsoleColor.Cyan);
            Logger.Print($"ALIVE:{Alive}");
            if (Alive)
            {
                Logger.Print($"HP:{HP}/{Card.MaxHP} MP:{MP}/{Card.MaxMP} Element:{Element}");
            }
            Effects.Print();
        }
    }

}
