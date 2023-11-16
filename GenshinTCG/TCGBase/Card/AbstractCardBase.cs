namespace TCGBase
{
    public abstract class AbstractCardBase
    {
        /// <summary>
        /// 注册时给出，仅限行动牌、角色牌、状态，默认为"minecraft"<br/>
        /// 也可以选择不注册，自己给出namespace
        /// </summary>
        public virtual string Namespace { get; internal set; }
        /// <summary>
        /// 卡牌的nameID(a-z+0-9),如"keqing114"
        /// </summary>
        public abstract string NameID { get; }
        public AbstractCardBase()
        {
            Namespace = "minecraft";
        }
    }
}
