namespace TCGBase
{
    public class CatalyzeField : AbstractCardPersistentEffect
    {
        public override string NameID => "catalyze";
        public override int MaxUseTimes => 2;

        public override PersistentTriggerDictionary TriggerDic => new()
        {
            {SenderTag.DamageIncrease.ToString(),(me,p,s,v)=>
                {
                    if (s.TeamID==me.TeamIndex && v is DamageVariable dv && dv.TargetIndex==me.CurrCharacter && (dv.Element==4||dv.Element==6))
                    {
                        dv.Damage++;
                        p.AvailableTimes--;
                    }
                }
            }
        };
    }
}
