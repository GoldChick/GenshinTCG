using Minecraft;

namespace TCGBase
{
    public abstract class AbstractCardFoodSingle : AbstractCardEvent, ICardFood
    {
        public override TargetDemand[] TargetDemands => new TargetDemand[]
        {
            new(TargetEnum.Character_Me,CanBeUsed)
        };

        public virtual int InitialUseTimes => MaxUseTimes;
        /// <summary>
        /// 如果可用次数<b>大于0</b>，代表有状态可使用，于是额外附加状态<br/>
        /// 否则没有额外状态
        /// </summary>
        public abstract int MaxUseTimes { get; }

        public int Variant => 0;

        public bool CustomDesperated => false;

        public virtual PersistentTriggerDictionary TriggerDic => new();


        /// <summary>
        /// 默认实现 [附属饱腹]+[附属AfterEatEffect](如果有)
        /// </summary>
        public override void AfterUseAction(PlayerTeam me, int[] targetArgs)
        {
            me.AddPersistent(new Full(), targetArgs[0]);
            if (MaxUseTimes > 0)
            {
                me.AddPersistent(this, targetArgs[0]);
            }
        }
        /// <summary>
        /// 默认条件：活着并且不饱腹
        /// </summary>
        public override bool CanBeUsed(PlayerTeam me, int[] targetArgs)
        {
            var c = me.Characters[targetArgs[0]];
            return c.Alive && !c.Effects.Contains("minecraft", "full");
        }

        public void Update<T>(PlayerTeam me, Persistent<T> persistent) where T : ICardPersistent
        {
            persistent.Data = null;
            persistent.AvailableTimes = int.Max(persistent.AvailableTimes, MaxUseTimes);
        }
        public virtual void OnDesperated(PlayerTeam me, int region)
        {
        }
    }
}
