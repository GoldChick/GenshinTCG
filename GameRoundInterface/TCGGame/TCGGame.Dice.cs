using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCGBase;

namespace TCGGame
{
    public class Dice
    {
        private ElementType elementType;

        public Dice()
        {
            elementType = RandomElementType();
        }
        public Dice(ElementType elementType)
        {
            this.elementType = elementType;
        }
        public Dice(int element)
        {
            elementType = (ElementType)element;
        }
        public ElementType GetElementType() => elementType;

        //TODO:随机数产生问题
        //不能一次产生多个
        public static ElementType RandomElementType()
        {
            Random rd = new Random();
            return (ElementType)rd.Next(0, 8);
        }
    }
}
