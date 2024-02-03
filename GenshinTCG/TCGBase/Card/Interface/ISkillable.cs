namespace TCGBase
{
    /// <summary>
    /// 分别表示[非技能/被动][A][E][Q]
    /// </summary>
    public enum SkillCategory
    {
        P,
        A,
        E,
        Q,
    }
    public interface ISkillable
    {
        public SkillCategory SkillCategory { get; }
    }
}
