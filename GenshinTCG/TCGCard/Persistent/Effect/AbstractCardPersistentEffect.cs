using TCGBase;

namespace TCGCard
{
    /// <summary>
    /// Assist和Summon相当于[自己]和Effect的结合
    /// </summary>
    public abstract class AbstractCardPersistentEffect : AbstractCardPersistent, IDamageSource
    {
        public override sealed string[] SpecialTags => throw new Exception("AbstractCardEffect:不应该访问一个普通effect的Tags!");
        /// <summary>
        /// 是否在角色/团队effect中可见<br/>
        /// 其实也是客户端only的东西
        /// </summary>
        public virtual bool Visible { get => true; }
        /// <summary>
        /// 默认为addition，表示[旋火轮]等上来不直接来源于角色，但仍然是我方造成的伤害<br/>
        /// 可以继承改为nowhere，表示自己受到的伤害，不能被对方增伤加伤，（仍然可以被[水牢][箓灵]等减抗加伤）
        /// </summary>
        public virtual DamageSource DamageSource => DamageSource.Addition;
    }
}
