using TCGGame;

namespace TCGAI
{
    /// <summary>
    /// 只是用来让外面的AI成功编译
    /// </summary>
    public interface IGameInfo
    {
        /// <summary>
        /// 获取对方的粗略信息
        /// </summary>
        public TeamReadonly GetTeamReadonly();
        /// <summary>
        /// 获取己方的详细信息
        /// </summary>
        public TeamDetailReadonly GetTeamDetailReadonly();
    }
}
