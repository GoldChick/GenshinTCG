namespace TCGBase
{
    public enum DiceType
    {
        Trival,
        Anemo,
        Geo,
        Electro,
        Dendro,
        Hydro,
        Pyro,
        Cyro
    }
    public interface Dice
    {
        DiceType GetDiceType();
    }
}
