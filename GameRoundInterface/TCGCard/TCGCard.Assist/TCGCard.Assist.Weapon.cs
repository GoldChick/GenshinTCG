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
        WeaponType GetWeaponType();//武器的类型
        int GetBaseDamage();//武器基础攻击
        int GetAdditionalDamage(IGameBase[] igame);//一定条件下额外攻击
        void GetAdditonalEffect(IGameBase[] igame);//一定条件下触发的额外效果
    }
}