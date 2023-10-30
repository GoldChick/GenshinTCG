namespace TCGBase
{
    public abstract class AbstractCardBase
    {
        /// <summary>
        /// 卡牌的nameID(a-z+_+0-9),如"keqing"
        /// </summary>
        public abstract string NameID { get; }
    }
}
