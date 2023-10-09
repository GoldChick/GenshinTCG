namespace TCGCard
{
    /// <summary>
    /// Assist和Summon相当于[自己]和Effect的结合
    /// </summary>
    public abstract class AbstractCardEffect : AbstractCardPersistent
    {
        public override string[] Tags =>throw new Exception("AbstractCardEffect:不应该访问一个普通effect的Tags!");
        /// <summary>
        /// 是否在角色/团队effect中可见<br/>
        /// 其实也是客户端only的东西
        /// </summary>
        public virtual bool Visible { get => true; }

    }
}
