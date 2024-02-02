﻿namespace TCGBase
{
    /// <summary>
    /// only for Element_Enchant<br/>
    /// 元素附魔，如果是a，则变为b
    /// </summary>
    public class PersistentElementEnchant : ITriggerable
    {
        private int _toward;
        private int _adddamage;
        /// <summary>
        /// 是否参与使用次数计数，如果是则会触发后减少使用次数
        /// </summary>
        private bool _decreaseAfterUse;

        public PersistentElementEnchant(int element, bool decreaseAfterUse = false, int adddamageIfEnchant = 0)
        {
            _toward = int.Clamp(element, 0, 7);
            _decreaseAfterUse = decreaseAfterUse;
            _adddamage = adddamageIfEnchant;
        }

        public string Tag => SenderTag.ElementEnchant.ToString();
        public void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
        {
            if (persitent.AvailableTimes > 0 && variable is DamageVariable dv && sender.TeamID == me.TeamIndex)
            {
                if ((persitent.PersistentRegion < 0 || persitent.PersistentRegion > 10 || me.CurrCharacter == persitent.PersistentRegion) && dv.DirectSource == DamageSource.Character)
                {
                    if (dv.Element == 0)
                    {
                        dv.Element = _toward;
                        dv.Damage += _adddamage;
                        if (_decreaseAfterUse)
                        {
                            persitent.AvailableTimes--;
                        }
                    }
                }
            }
        }
    }
}
