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
        public override PersistentTriggerDictionary TriggerDic => new() {
            {TCGBase.Tags.SenderTags.AFTER_ANY_ACTION, new HeavyAttackTrigger()} ,
             };
        public class HeavyAttackTrigger : PersistentTrigger
        {
            public void Trigger(PlayerTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
            {
                //TODO:调用错误
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
