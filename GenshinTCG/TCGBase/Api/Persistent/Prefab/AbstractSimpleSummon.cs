﻿namespace TCGBase
{
    /// <summary>
    /// 结束时，对对方出战角色造成a点b元素伤害，初始可用次数=最大可用次数=c<br/>
    /// </summary>
    public abstract class AbstractSimpleSummon : AbstractCardPersistentSummon
    {
        public override int MaxUseTimes { get; }
        private readonly int _damage;
        private readonly int _element;
        public AbstractSimpleSummon(int element, int damage, int maxusetimes)
        {
            _damage = damage;
            _element = element;
            MaxUseTimes = maxusetimes;
        }

        public override PersistentTriggerDictionary TriggerDic => new()
        {
            { SenderTag.RoundOver.ToString(),(me, p, s, v) => { me.Enemy.Hurt(new(_element, _damage, 0), this); p.AvailableTimes --; }}
        };
    }
}