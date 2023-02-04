using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCGBase;

namespace TCGGame
{
    /// <summary>
    /// 对方的攻击直接作用于Team上
    /// 然后再进行细分
    /// </summary>
    public class Team
    {
        public Side side;
        public List<Effect> effects;
        public Queue<IEventBase> events;

        public List<Character> characters;

        private int currCharacter;

        public int CurrCharacter { get => currCharacter; set => currCharacter = value; }

        public void Hurt(IDamage damage)
        {
            characters[damage.GetTargetId()].Hurt(effects);
        }
        public void Switch(int id)
        {
            //Bus.Instance().Post(side);
            if (currCharacter != id)
            {
                currCharacter = id;
            }
        }
    }
}
