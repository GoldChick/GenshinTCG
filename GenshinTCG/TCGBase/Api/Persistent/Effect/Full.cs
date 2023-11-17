namespace TCGBase
{
    public class Full : AbstractCardPersistent
    {
        public override string NameID => "full";
        public override int MaxUseTimes => 1;

        public override PersistentTriggerDictionary TriggerDic => new()
        {
            { SenderTag.RoundOver,(me,p,s,v)=>p.AvailableTimes--}
        };
    }
}
