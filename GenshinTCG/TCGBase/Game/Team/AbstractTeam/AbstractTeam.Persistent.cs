namespace TCGBase
{
    public partial class AbstractTeam
    {
        public virtual void AddSupport(AbstractCardSupport support, int replace = -1)
        {
        }
        public void AddSummon(AbstractCardSummon summon) => AddSummon(1, summon);
        public virtual void AddSummon(int num, params AbstractCardSummon[] summons)
        {
        }

        /// <summary>
        /// effect按照 (curr->curr+1->curr+2->...)角色=>团队=>召唤物=>支援区 的顺序结算<br/>
        /// </summary>
        public virtual void EffectTrigger(AbstractSender sender, AbstractVariable? variable = null)
        {
        }
        /// <summary>
        /// 增加一个effect，只在PlayerTeam中有效
        /// IEffect -1:团队 0-(characters.count-1):个人
        /// </summary>
        /// <param name="bind">绑定在某个其他persistent上供检测，只对出战状态和角色状态有效</param>
        public virtual void AddPersistent(AbstractCardPersistent per, int target = -1, AbstractPersistent? bind = null)
        {

        }
    }
}
