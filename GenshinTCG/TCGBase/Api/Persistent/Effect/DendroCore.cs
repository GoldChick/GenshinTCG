namespace TCGBase
{
    public class DendroCore : AbstractCardPersistent
    {
        public override string NameID => "dendro_core";
        public override int MaxUseTimes => 1;

        public override PersistentTriggerDictionary TriggerDic => new()
        {
            { SenderTag.DamageIncrease.ToString(),new PersistentTrigger((me,p,s,v)=>
                {
                    if (s.TeamID==me.TeamIndex &&v is DamageVariable dv && (dv.Element==3 || dv.Element==4))
                    {
                        dv.Damage+=2;
                        p.AvailableTimes--;
                    }
                }
             )}
        };

    }
}
