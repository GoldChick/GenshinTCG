﻿namespace TCGBase
{
    public class Full : AbstractCardPersistentEffect
    {
        public override string TextureNameID => PersistentTextures.Full;
        public override int MaxUseTimes => 1;

        public override PersistentTriggerDictionary TriggerDic => new()
        {
            { SenderTag.RoundOver,(me,p,s,v)=>p.AvailableTimes--}
        };
    }
}