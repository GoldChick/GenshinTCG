namespace TCGCard
{
    /// <summary>
    /// Assist和Summon相当于[自己]和Effect的结合
    /// </summary>
    public interface IEffect:IPersistent
    {
        /// <summary>
        /// 是否在角色/团队effect中可见
        /// </summary>
        public bool Visible { get; }
    }
}
