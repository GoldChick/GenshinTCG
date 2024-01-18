﻿using Minecraft;

namespace TCGBase
{
    public abstract class AbstractCardFoodSingle : AbstractCardEvent, ICardFood
    {
        public override TargetDemand[] TargetDemands => new TargetDemand[]
        {
            new(TargetEnum.Character_Me,CanBeUsed)
        };
        /// <summary>
        /// 如果triggerdic中有内容，就添加状态给制定目标
        /// </summary>
        public override PersistentTriggerDictionary TriggerDic => new();
        /// <summary>
        /// 默认实现 [附属饱腹]+[附属AfterEatEffect](如果有)
        /// </summary>
        public override void AfterUseAction(PlayerTeam me, int[] targetArgs)
        {
            me.AddPersistent(new Effect_Full(), targetArgs[0]);
            if (TriggerDic.Any())
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
    }
}
