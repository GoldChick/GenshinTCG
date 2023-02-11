using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCGBase;
using TCGGame;

namespace TCGCard
{
    public enum WeaponType
    {
        Other,
        Sword,
        BigSword,
        LongWeapon,
        Catalyst,
        Bow
    }
    public interface ICardWeapon : ICardAssist
    {
        public WeaponType WeaponType { get; }
        public int BaseDamage { get; }

        int GetAdditionalDamage(IGameBase[] igame);//一定条件下额外攻击
        void GetAdditonalEffect(IGameBase[] igame);//一定条件下触发的额外效果
    }
}