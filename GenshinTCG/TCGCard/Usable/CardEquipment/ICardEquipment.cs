namespace TCGCard
{
    public interface ICardEquipment : ICardOwnalbe
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
