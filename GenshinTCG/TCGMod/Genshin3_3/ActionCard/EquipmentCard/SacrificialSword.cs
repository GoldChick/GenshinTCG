using TCGCard;

namespace Genshin3_3
{
    public class SacrificialSword : ICardWeapon
    {
        public string WeaponType => TCGBase.Tags.CardTags.WeaponTags.SWORD;

        public int MaxNumPermitted => 2;

        public string NameID => "sacrificial_sword";

        public HashSet<string> Tags => throw new NotImplementedException();

        public bool SameDice => throw new NotImplementedException();

        public Dictionary<string, int> Costs => throw new NotImplementedException();

        public void AfterUseAction()
        {
            throw new NotImplementedException();
        }

        public bool CanBeArmed()
        {
            throw new NotImplementedException();
        }

        public bool CanBeUsed()
        {
            throw new NotImplementedException();
        }
    }
}
