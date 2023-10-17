using TCGBase;
using TCGCard;
using TCGGame;

namespace TCGMod
{
    /// <summary>
    /// only for Element_Enchant<br/>
    /// 元素附魔，如果是a，则变为b
    /// </summary>
    public class PersistentElementEnchant : PersistentTrigger
    {
        private int _before;
        private int _toward;
        /// <summary>
        /// 是否参与使用次数计数，如果是则会触发后减少使用次数
        /// </summary>
        private bool _counter;
        public PersistentElementEnchant(int before, int toward, bool counter = false)
        {
            _before = int.Clamp(before, 0, 7);
            _toward = int.Clamp(toward, 0, 7);
            _counter = counter;
        }

        public PersistentElementEnchant(int toward, bool counter = false)
        {
            _before = 0;
            _toward = int.Clamp(toward, 0, 7);
            _counter = counter;
        }
        public override void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
        {
            if (persitent.AvailableTimes > 0 && variable is DamageVariable dv && sender.TeamID == me.TeamIndex)
            {
                if (persitent is not PersonalEffect pe || me.CurrCharacter == pe.Owner)
                {
                    if (dv.Element == _before)
                    {
                        dv.Element = _toward;
                        if (_counter)
                        {
                            persitent.AvailableTimes--;
                        }
                    }
                }
            }
        }
    }
}
