using System.Collections.Generic;
using TCGCard;
using TCGGame;

namespace TCGBase
{
    public enum TargetType
    {
        Curr,
        Left,
        Right
    }
    public class Damage
    {
        private int damageNum;
        private int targetId;

        public bool pierce;
        public ElementType elementType;
        public List<Effect> effects;
        public ICardBase card;
        public int DamageNum
        {
            get => damageNum;
            set
            {
                damageNum = value;
            }
        }

        public int TargetId { get => targetId; set => targetId = value; }
    }
}
