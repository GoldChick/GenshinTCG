using TCGBase;

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
        public override PersistentTriggerDictionary TriggerDic => new() 
        {
            {SenderTag.AfterAnyAction, new HeavyAttackTrigger()} ,
        };
        public class HeavyAttackTrigger : PersistentTrigger
        {
            public override void Trigger(PlayerTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
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
