using System.Drawing;
//################################################################
//仅用于客户端使用
//不含有任何游戏机制
//只提供游戏资源
//################################################################
namespace TCGCard
{
    /// <summary>
    /// 仅在客户端有效的数据
    /// <br/>
    /// 包括卡牌显示的名字，描述，图像(或者图标)
    /// </summary>
    public interface ICardClient
    {
        /// <summary>
        /// 显示的卡牌名字
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// 显示的卡牌说明
        /// </summary>
        public string Description { get; }
        /// <summary>
        /// 卡牌的图像，对尺寸有要求
        /// <br/>
        /// 人物卡/辅助卡等应该为{TODO}
        /// <br/>
        /// 状态卡(其实已经不能叫卡了)等应为{TODO}
        /// </summary>
        public Bitmap GetImageBmp();
    }
    /// <summary>
    /// 不应该直接继承这个类，否则不会被检测到
    /// <br/>
    /// 继承这个抽象类的<b>子类</b>以提供卡牌的相关信息
    /// <br/>
    /// 每个这样的子类只能够被继承一次，否则可能产生问题
    /// </summary>
    public abstract class CardSupplier
    {
        /// <summary>
        /// 推荐是通过switch语句
        /// <br/>
        /// 匹配不到时可以放心直接返回null
        /// </summary>
        public abstract ICardClient GetCard(string id);
    }
    public abstract class CharacterSupplier : CardSupplier
    {

    }
    public abstract class SkillSupplier : CardSupplier
    {

    }
    public abstract class AssistSupplier : CardSupplier
    {

    }
    public abstract class EffectSupplier : CardSupplier
    {

    }
}



