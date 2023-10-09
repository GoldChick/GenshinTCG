namespace TCGCard
{
    public abstract class AbstractCardEquipment : AbstractCardAction
    {
    }
    public abstract class AbstractCardWeapon : AbstractCardEquipment
    {
        public abstract string WeaponType { get; }
        
    }
    public abstract class ICardArtifact :AbstractCardEquipment
    {

    }
}
