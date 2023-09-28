using TCGBase;
using TCGCard;
using TCGGame;

namespace TCGMod
{
    /// <summary>
    /// only for Element_Enchant<br/>
    /// 元素附魔，如果是a，则变为b
    /// </summary>
    public class PersistentElementEnchant : IPersistentTrigger
    {
        private int _before;
        private int _toward;
        public PersistentElementEnchant(int before, int toward)
        {
            _before = int.Clamp(before, 0, 7);
            _toward = int.Clamp(toward, 0, 7);
        }

        public PersistentElementEnchant(int toward)
        {
            _before = 0;
            _toward = int.Clamp(toward, 0, 7);
        }
        public void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
        {
            if (variable is DamageVariable dv)
            {
                if (dv.Element == _before)
                {
                    dv.Element = _toward;
                }
            }
        }
    }
}
