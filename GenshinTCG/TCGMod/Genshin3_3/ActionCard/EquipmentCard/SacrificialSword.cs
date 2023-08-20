using TCGCard;
using TCGGame;

namespace Genshin3_3
{
    public class SacrificialSword : ICardWeapon
    {
        public string WeaponType => TCGBase.Tags.CardTags.WeaponTags.SWORD;

        public int MaxNumPermitted => 2;

        public string NameID => "sacrificial_sword";

        public string[] Tags => throw new NotImplementedException();

        public int[] Costs => throw new NotImplementedException();

        public bool CostSame => throw new NotImplementedException();


        public void AfterUseAction()
        {
            throw new NotImplementedException();
        }

        public bool CanBeArmed()
        {
            throw new NotImplementedException();
        }

        public bool CanBeUsed(AbstractGame game, int meIndex)
        {
            throw new NotImplementedException();
        }
    }
}
