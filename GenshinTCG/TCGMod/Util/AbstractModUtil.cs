using TCGRule;

namespace TCGMod
{
    /// <summary>
    /// 每个提供卡片的dll都要有唯一一个实现IUtils的类来提供此dll的信息
    /// mod的版本使用[AssemblyVersion]指定
    /// </summary>
    public abstract class AbstractModUtil
    {
        /// <summary>
        /// 此MOD独有的namespace，用于与其他mod作区分(a-z+_+0-9)
        /// 不可重复，否则不能正常加载
        /// </summary>
        public abstract string NameSpace { get; }
        /// <summary>
        /// 对于此mod的描述
        /// 随便写写就行
        /// </summary>
        public abstract string Description { get; }
        /// <summary>
        /// 作者的名字
        /// 推荐大家诚实一点不要冒充别人
        /// </summary>
        public abstract string Author { get; }

        /// <summary>
        /// 依赖项的namespace（作为某些mod的子mod时使用）
        /// 具体的呈现格式为"minecraft:keqing"中的"minecraft"
        /// </summary>
        public abstract string[] GetDependencies();
        /// <summary>
        /// 注册各种卡牌、规则
        /// </summary>
        public abstract AbstractRegister GetRegister();
    }

}