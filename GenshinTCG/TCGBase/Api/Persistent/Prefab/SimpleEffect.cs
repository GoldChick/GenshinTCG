namespace TCGBase
{
    /// <summary>
    /// 只是用来给一些触发提供占位符，如[本大爷还没有输]\[霜华矢]
    /// </summary>
    public class SimpleEffect : AbstractCardPersistentEffect
    {
        public override string TextureNameSpace { get; }
        public override string TextureNameID { get; }
        public override int MaxUseTimes => 1;
        public override PersistentTriggerDictionary TriggerDic { get; }
        private void SetDeActive(PlayerTeam me, AbstractPersistent p, AbstractSender s, AbstractVariable? v) => p.Active = false;
        public SimpleEffect(string textureNameid)
        {
            TextureNameSpace = "minecraft";
            TextureNameID = textureNameid;
            TriggerDic = new();
        }
        public SimpleEffect(string textureNamespace, string textureNameid, params SenderTag[] fades) : this(textureNameid, fades)
        {
            TextureNameSpace = textureNamespace;
        }
        /// <param name="fades">在提供的sendertag触发后消失</param>
        public SimpleEffect(string textureNameid, params SenderTag[] fades)
        {
            TextureNameSpace = "minecraft";
            TextureNameID = textureNameid;
            TriggerDic = new();
            Array.ForEach(fades, st => TriggerDic.Add(st, SetDeActive));
        }
        public SimpleEffect(string textureNamespace, string textureNameid, params string[] fades) : this(textureNameid, fades)
        {
            TextureNameSpace = textureNamespace;
        }
        /// <param name="fades">在提供的sendertag触发后消失</param>
        public SimpleEffect(string textureNameid, params string[] fades)
        {
            TextureNameSpace = "minecraft";
            TextureNameID = textureNameid;
            TriggerDic = new();
            Array.ForEach(fades, st => TriggerDic.Add(st, SetDeActive));
        }

    }
}
