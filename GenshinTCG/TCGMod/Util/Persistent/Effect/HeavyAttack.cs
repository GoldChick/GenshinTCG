using TCGBase;
using TCGCard;
using TCGGame;

namespace TCGMod
{
    /// <summary>
    /// 游戏开始时，为双方附属
    /// </summary>
    public class HeavyAttack : AbstractCardPersistentEffect
    {
        public override string NameID => "heavy_attack";

        public override int MaxUseTimes => 1;

        public override bool DeleteWhenUsedUp => false;
        public override Dictionary<string, IPersistentTrigger> TriggerDic => new() {
            {TCGBase.Tags.SenderTags.AFTER_ANY_ACTION, new HeavyAttackTrigger()} ,
             };
        public class HeavyAttackTrigger : IPersistentTrigger
        {
            public void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
            {
                if (me.GetDiceNum() % 2 == 0)
                {
                    persitent.AvailableTimes = 1;
                }
                else
                {
                    persitent.AvailableTimes = 0;
                }
            }
        }
    }
}
