namespace TCGBase
{
    /// <summary>
    /// Assist和Summon相当于[自己]和Effect的结合
    /// </summary>
    public abstract class AbstractCardPersistentTalent : AbstractCardPersistent
    {
        public override int MaxUseTimes => 0;
        public override bool DeleteWhenUsedUp => false;
        /// <summary>
        /// 要覆写的skill
        /// </summary>
        public abstract int Skill { get; }
        /// <summary>
        /// 使用后发生什么，<b>覆盖</b>原有技能<br/>
        /// targetargs是可能的自定义Additionaltargetargs(需要自己维护)<br/><br/>
        /// <b>对于被动技能targetargs[0]表示teamid，并且没有additionaltargetargs</b>
        /// </summary>
        public virtual void AfterUseAction(PlayerTeam me, Character c, int[]? targetArgs = null)
        {
            c.Card.Skills[Skill % c.Card.Skills.Length].AfterUseAction(me, c, targetArgs);
        }
        /// <summary>
        /// 默认的TriggerDic为空，但也不排除特殊情况，如[迪西雅]天赋
        /// </summary>
        public override PersistentTriggerDictionary TriggerDic => new();
    }
}
