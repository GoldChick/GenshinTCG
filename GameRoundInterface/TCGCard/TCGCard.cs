using System.Drawing;
//################################################################
//仅用于客户端使用
//不含有任何游戏机制
//只提供游戏资源
//################################################################
namespace TCGCard
{
    public interface ICardClient : ICardBase
    {
        /// <summary>
        /// 显示的名字
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// 显示的卡牌说明
        /// </summary>
        public string Description { get; }
        /// <summary>
        /// 卡牌的图像
        /// </summary>
        public Bitmap GetImageBmp();
    }
}



