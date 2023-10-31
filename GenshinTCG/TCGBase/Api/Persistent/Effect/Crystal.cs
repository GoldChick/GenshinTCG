namespace TCGBase
{
    public class Crystal : AbstractCardPersistentEffect
    {
        public override string TextureNameID => PersistentTextures.Shield_Yellow;
        public override int InitialUseTimes => 1;
        public override int MaxUseTimes => 2;

        public override PersistentTriggerDictionary TriggerDic => new()
        {
            { SenderTag.HurtDecrease.ToString(),new PersistentYellowShield()}
        };
    }
}
