namespace TCGBase
{
    public abstract class AbstractCardFood : AbstractCardEvent, ITargetSelector
    {
        /// <summary>
        /// 吃了之后会上什么buff
        /// </summary>
        public abstract AbstractCardPersistentEffect? AfterEatEffect { get; }
        public TargetEnum[] TargetEnums => new TargetEnum[] { TargetEnum.Character_Me };
        public override bool CanBeUsed(PlayerTeam me, int[] targetArgs)
        {
            var c = me.Characters[targetArgs[0]];
            return c.Alive && !c.Effects.Contains(PersistentTextures.Full);
        }
    }
}
