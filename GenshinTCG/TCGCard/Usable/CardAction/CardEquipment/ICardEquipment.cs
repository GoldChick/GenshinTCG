namespace TCGCard
{
    public interface ICardEquipment : ICardAction
    {
    }
    public interface ICardWeapon : ICardEquipment
    {
        public string WeaponCategory { get; }
    }
    public interface ICardArtifact:ICardEquipment
    {

    }
}
