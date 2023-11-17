namespace TCGBase
{
    public abstract class AbstractCardBase
    {
        public string Namespace => (GetType().Namespace ?? "minecraft").ToLower();
        /// <summary>
        /// 卡牌的nameID(a-z+0-9),如"keqing114"
        /// </summary>
        public virtual string NameID { get => GetType().ToString().ToLower(); }
    }
}
