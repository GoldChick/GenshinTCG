namespace TCGBase
{

    /// <summary>
    /// 用于表明是直接伤害，还是间接伤害(扩散、超导等引发的额外伤害)
    /// </summary>
    public enum DamageSource
    {
        Direct,
        Indirect
    }
    public enum TargetTeam
    {
        Enemy,
        Me
    }
    public enum TargetArea
    {
        TargetOnly,
        TargetExcept
    }
}
