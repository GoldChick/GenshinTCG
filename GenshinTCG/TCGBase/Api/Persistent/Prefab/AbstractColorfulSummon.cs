namespace TCGBase
{
    /// <summary>
    /// 可染色(借助于persistent.Data)召唤物，默认元素为风，且[我方]引发扩散反应后染色(离场前限一次)
    /// </summary>
    public abstract class AbstractColorfulSummon : AbstractCardSummon
    {
        public override int MaxUseTimes { get; }
        protected readonly int _damage;
        protected readonly int _init_element;
        /// <summary>
        /// 默认是[我方]扩散之后变颜色
        /// </summary>
        public AbstractColorfulSummon(int damage, int maxusetimes, bool customRoundOver = false, bool customChangeColor = false, int initial_element = 7)
        {
            _damage = int.Min(damage, 1);
            _init_element = initial_element;
            MaxUseTimes = maxusetimes;
            TriggerDic = new();

            if (!customRoundOver)
            {
                TriggerDic.Add(SenderTag.RoundOver, (me, p, s, v) => me.Enemy.Hurt(new(p.Data is int element ? element : _init_element, _damage, 0), this, () => p.AvailableTimes--));
            }
            if (!customChangeColor)
            {
                TriggerDic.Add(SenderTag.AfterHurt, (me, p, s, v) =>
                {
                    if (p.Data == null && me.TeamIndex == s.TeamID && s is NoDamageHurtSender hs && hs.Reaction == ReactionTags.Swirl)
                    {
                        p.Data = hs.InitialElement;
                        Variant += 10 * hs.InitialElement;
                    }
                });
            }
        }
        public override PersistentTriggerDictionary TriggerDic { get; }
        public override void Update<T>(PlayerTeam me, Persistent<T> persistent)
        {
            base.Update(me, persistent);
            Variant %= 10;
        }
    }
}
