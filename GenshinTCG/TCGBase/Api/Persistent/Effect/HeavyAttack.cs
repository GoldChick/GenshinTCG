using TCGBase;

namespace Minecraft
{
    /// <summary>
    /// [我方行动开始前]时，为我方消除，再附属
    /// </summary>
    public class HeavyAttack : AbstractCardPersistent
    {
        public override string NameID => "heavy_attack";
        public override int MaxUseTimes => 1;
        public override PersistentTriggerDictionary TriggerDic => new()
        {
            { SenderTag.RoundMeStart, (me,p,s,v)=>p.Active=me.Dices.Count%2==0}
        };
    }
}
