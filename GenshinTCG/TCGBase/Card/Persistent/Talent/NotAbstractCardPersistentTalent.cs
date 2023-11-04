namespace TCGBase
{
    /// <summary>
    /// Assist和Summon相当于[自己]和Effect的结合
    /// </summary>
    public class CardPersistentTalent : AbstractCardPersistent
    {
        public override int MaxUseTimes => 0;
        /// <summary>
        /// 天赋牌默认不会过期
        /// </summary>
        public override bool CustomDesperated => false;
        /// <summary>
        /// 要覆写的skill，默认不覆写，即没有特殊效果<br/>
        /// 如果要覆写技能，请override下面的AfterUseAction()<br/>
        /// 当然，你也可以在技能的地方做关于天赋牌的检测
        /// </summary>
        public virtual int Skill { get => 0; }
        /// <summary>
        /// 使用index为Skill的技能后发生什么，<b>覆盖</b>原有技能<br/>
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
