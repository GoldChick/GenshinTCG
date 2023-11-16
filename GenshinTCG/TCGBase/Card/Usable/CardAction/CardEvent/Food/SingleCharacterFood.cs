namespace TCGBase
{
    public abstract class SingleCharacterFood : AbstractCardEvent, ICardFood, ITargetSelector
    {
        public TargetEnum[] TargetEnums => new TargetEnum[] { TargetEnum.Character_Me };

        public abstract AbstractCardPersistentEffect? AfterEatEffect { get; }

        /// <summary>
        /// 默认实现 [附属饱腹]+[附属AfterEatEffect](如果有)
        /// </summary>
        public override void AfterUseAction(PlayerTeam me, int[] targetArgs)
        {
            me.AddPersistent(new Full(), targetArgs[0]);
            if (AfterEatEffect != null)
            {
                me.AddPersistent(AfterEatEffect, targetArgs[0]);
            }
        }
        /// <summary>
        /// 默认条件：活着并且不饱腹
        /// </summary>
        public override bool CanBeUsed(PlayerTeam me, int[] targetArgs)
        {
            var c = me.Characters[targetArgs[0]];
            return c.Alive && !c.Effects.Contains("full");
        }
    }
}
