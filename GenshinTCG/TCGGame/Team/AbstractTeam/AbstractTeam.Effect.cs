using TCGBase;

namespace TCGGame
{
    public abstract partial class AbstractTeam
    {
        /// <summary>
        /// 在某一次所有的结算之后，清除not active的effect
        /// </summary>
        /// <returns>删除的effect总数量</returns>
        private int EffectUpdate()
        {
            int sum = Characters.Select(c => c.Effects.Update()).Sum();
            sum += Effects.Update();
            sum += Summons.Update();
            sum += Supports.Update();
            return sum;
        }
        /// <summary>
        /// effect按照 0-N角色=>团队=>召唤物=>支援区 的顺序结算
        /// </summary>
        public void EffectTrigger(AbstractGame game, int meIndex, AbstractSender sender, AbstractVariable? variable = null)
        {
            Array.ForEach(Characters, c => c.EffectTrigger(game, meIndex, sender, variable));

            Effects.EffectTrigger(game, meIndex, sender, variable);
            Summons.EffectTrigger(game, meIndex, sender, variable);
            Supports.EffectTrigger(game, meIndex, sender, variable);

            EffectUpdate();
            //TODO:test
            //Logger.Print($"Team{TeamIndex}清除了{EffectUpdate()}个耗尽的effect！此时的sender为{sender.SenderName}");
        }
    }
}
