//namespace TCGBase
//{
//    public abstract class AbstractCardFoodSingle : AbstractCardAction, ICardFood
//    {
//        public override TargetDemand[] TargetDemands => new TargetDemand[]
//        {
//            new(TargetEnum.Character_Me,CanBeUsed)
//        };
//        /// <summary>
//        /// 默认实现 [附属饱腹]+[附属AfterEatEffect](如果有)
//        /// </summary>
//        public override void AfterUseAction(PlayerTeam me, int[] targetArgs)
//        {
//            //TODO: full
//            //me.AddPersistent(new Effect_Full(), targetArgs[0]);
//            if (TriggerableList.Any())
//            {
//                me.AddEffect(this, targetArgs[0]);
//            }
//        }
//        /// <summary>
//        /// 默认条件：活着并且不饱腹
//        /// </summary>
//        public override bool CanBeUsed(PlayerTeam me, int[] targetArgs)
//        {
//            var c = me.Characters[targetArgs[0]];
//            return c.Alive && !c.Effects.Contains("minecraft", "full");
//        }
//    }
//}
