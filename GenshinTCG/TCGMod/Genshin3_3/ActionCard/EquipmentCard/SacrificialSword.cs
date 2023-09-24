using TCGCard;
using TCGGame;

namespace Genshin3_3
{
    public class SacrificialSword : ICardWeapon, ITargetSelector
    {
        public string WeaponType => TCGBase.Tags.CardTags.WeaponTags.SWORD;

        public int MaxNumPermitted => 2;

        public string NameID => "sacrificial_sword";

        public string[] Tags => new string[] { WeaponType };

        public int[] Costs => new int[] { 1 };

        public bool CostSame => false;

        public TargetEnum[] TargetEnums => new TargetEnum[] { TargetEnum.Character_Me };

        public void AfterUseAction(AbstractGame game, int meIndex)
        {
            throw new NotImplementedException();
        }

        public bool CanBeArmed() => true;

        public bool CanBeUsed(AbstractGame game, int meIndex) => true;
    }
}
