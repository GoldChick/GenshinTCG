namespace TCGCard
{
    public interface ICardEquipment : ICardAction
    {
    }
    public interface ICardWeapon : ICardEquipment
    {
        public string WeaponType { get; }
    }
    public interface ICardArtifact:ICardEquipment
    {

    }
}
