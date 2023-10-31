namespace TCGBase
{
    /// <summary>
    /// 结束时，对对方出战角色造成a点b元素伤害，初始可用次数=最大可用次数=c
    /// </summary>
    public class SimpleSummon : AbstractCardPersistentSummon
    {
        public override string TextureNameSpace { get; }
        public override string TextureNameID { get; }
        public override int MaxUseTimes { get; }
        private readonly int _damage;
        private readonly int _element;
        public SimpleSummon(string textureNamespace, string textureNameid, int element, int damage, int maxusetimes) : this(textureNameid, element, damage, maxusetimes)
        {
            TextureNameSpace = textureNamespace;
        }
        public SimpleSummon(string textureNameid, int element, int damage, int maxusetimes)
        {
            TextureNameSpace = "minecraft";
            TextureNameID = textureNameid;
            _damage = damage;
            _element = element;
            MaxUseTimes = maxusetimes;
        }

        public override PersistentTriggerDictionary TriggerDic => new()
        {
            { SenderTag.RoundOver.ToString(),(me, p, s, v) => { me.Enemy.Hurt(new(_element, _damage, 0), this); p.AvailableTimes --; }}
        };
    }
}
