namespace TCGBase
{
    public abstract class AbstractCardBase
    {
        /// <summary>
        /// 注册时给出，仅限行动牌、角色牌，默认为"minecraft"
        /// </summary>
        public string Namespace { get; internal set; }
        /// <summary>
        /// 卡牌的nameID(a-z+_+0-9),如"keqing"
        /// </summary>
        public abstract string NameID { get; }
        public AbstractCardBase()
        {
            Namespace = "minecraft";
        }
    }
}
