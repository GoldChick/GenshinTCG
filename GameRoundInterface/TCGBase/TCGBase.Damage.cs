using System.Collections.Generic;
using TCGCard;
using TCGGame;

namespace TCGBase
{
    public class Damage
    {
        public enum TargetPos
        {
            Curr,
            Left,
            Right
        }
        private int damageNum;
        private int targetId;

        public bool pierce;
        public ElementType elementType;
       // public List<Effect> effects=new();
        public ICardBase origin;
        public int TargetId { get => targetId; }

        /// <summary>
        /// targetId 为相对位置
        /// </summary>
        /// <param name="targetId">相对位置，只能为-1,0,1</param>
        public Damage(int damageNum, TargetPos targetPos, ICardBase origin = null, bool pierce = false,
            ElementType elementType = ElementType.Trival
            //, 
           // List<Effect> effects = null
            )
        {
            this.damageNum = damageNum;
            this.targetId = targetPos switch
            {
                TargetPos.Curr => 0,
                TargetPos.Left => -1,
                TargetPos.Right => 1,
                _ => throw new System.Exception("TCGBase.Damage.TargetId went wrong when gaming. Possible cause: Wrong Target ID!")
            };
            this.pierce = pierce;
            this.elementType = elementType;
          //  this.effects = effects;
            this.origin = origin;
        }
        public int DamageNum
        {
            get => damageNum;
            set
            {
                damageNum = value;
            }
        }
    }
}
