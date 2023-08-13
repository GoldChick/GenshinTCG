using TCGBase;
using TCGCard;

namespace TCGGame
{
    public class Character : AbstractPersistent<ICardCharacter>
    {
        private int _hp;
        public Equipment Weapon { get; internal set; }
        public Equipment Artifact { get; internal set; }
        public Equipment Talent { get; internal set; }

        public List<Effect> Effects { get; private set; }

        public int HP
        {
            get { return _hp; }
            set { _hp = int.Clamp(value, 0, Card.MaxHP); }
        }
        public int MP;
        public bool Alive;
        public Character(ICardCharacter character)
        {
            Card = character;

            Effects = new(); //TODO:Default Effect

            HP = Card.MaxHP;
            HP = Card.MaxMP;
            Alive = true;
        }
        public override void EffectTrigger(AbstractGame game, int meIndex, AbstractSender sender, AbstractVariable? variable)
        {
            Weapon?.EffectTrigger(game, meIndex, sender, variable);
            Artifact?.EffectTrigger(game, meIndex, sender, variable);
            Talent?.EffectTrigger(game, meIndex, sender, variable);
            foreach (var e in Effects)
            {
                e.EffectTrigger(game,meIndex,sender,variable);
            }
        }
    }

}
