using System.Collections.Generic;
//################################################################
//Server和Client都要使用的Lib
//################################################################
namespace TCGCard
{
    /// <summary>
    /// 每个提供卡片的dll都要有唯一一个实现IUtils的类来提供此dll的信息
    /// mod的版本使用[AssemblyVersion]指定
    /// </summary>
    public interface IUtils
    {
        /// <summary>
        /// 此MOD独有的namespace，用于与其他mod作区分
        /// 不可重复，否则不能正常加载
        /// </summary>
        public string NameSpace { get; }
        /// <summary>
        /// 对于此mod的描述
        /// 随便写写就行
        /// </summary>
        public string Description { get; }
        /// <summary>
        /// 作者的名字
        /// 推荐大家诚实一点不要冒充别人
        /// </summary>
        public string Author { get; }
        /// <summary>
        /// 依赖项的namespace（作为某些mod的子mod时使用）
        /// 具体的呈现格式为"minecraft:keqing"中的"minecraft"
        /// </summary>
        public List<string> Dependencies { get; }
    }
    public interface ICardBase
    {
        /// <summary>
        /// 并不是卡牌的名字
        /// 而是卡牌对应的id
        /// 如"minecraft:keqing"中的"keqing"
        /// </summary>
        public string NameID { get; }
    }
    /// <summary>
    /// 卡片具有的属性词条
    /// 如[地区][种类]
    /// </summary>
    public class CardAttributeBase
    {
        /// <summary>
        /// 属性词条的种类
        /// 如[国家_璃月]中的国家
        /// </summary>
        public string NameSpace { get; }
        /// <summary>
        /// 词条名字
        /// 如[国家_璃月]中的璃月
        /// </summary>
        public string Name { get; }
    }

}



