namespace TCGBase
{
    public abstract class SingleCharacterFood : AbstractFood
    {
        /// <summary>
        /// 默认实现 [附属饱腹]+[附属AfterEatEffect](如果有)
        /// </summary>
        /// <param name="me"></param>
        /// <param name="targetArgs"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void AfterUseAction(PlayerTeam me, int[]? targetArgs = null)
        {
            me.AddPersistent(new Full(), targetArgs[0]);
            if (AfterEatEffect != null)
            {
                me.AddPersistent(AfterEatEffect, targetArgs[0]);
            }
        }
        public override bool CanBeUsed(PlayerTeam me, int[]? targetArgs = null)
        {
            var c = me.Characters[targetArgs[0]];
            return c.HP < c.Card.MaxHP && !c.Effects.Contains(PersistentTextures.Full);
        }
    }
}
