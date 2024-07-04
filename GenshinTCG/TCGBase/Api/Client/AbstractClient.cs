namespace TCGBase
{
    /// <summary>
    /// 尽管名字叫Client，实际上是在服务端运行的<br/>
    /// 其实更应该是Server与Client之间交互的通道，只是想不起来应该叫什么名字了
    /// </summary>
    public abstract class AbstractClient
    {
        private PlayerTeam _me;
        public ReadonlyGame Game { get; protected set; }
        /// <summary>
        /// 服务端=>客户端=>服务端
        /// 游戏开始前传入卡组
        /// </summary>
        public abstract ServerPlayerCardSet RequestCardSet();
        /// <summary>
        /// 服务端=>客户端=>服务端
        /// 游戏进行中调用索取对应行动
        /// </summary>
        public abstract NetEvent RequestEvent(OperationType demand);
        /// <summary>
        /// 服务端=>客户端
        /// 表示正在向对方request需要的event
        /// </summary>
        public virtual void RequestEnemyEvent(OperationType demand) { }
        public List<TargetEnum> GetCardTargetEnums(int cardindex) => _me.GetCardTargetEnums(cardindex);
        /// <summary>
        /// 对于cardindex这张卡，已经有already_params这些选中的参数了，但是还需要选择更多
        /// </summary>
        /// <returns>可供选择的对象在其区域的index们</returns>
        public List<int> GetCardNextValidTargets(int cardindex, int[] already_params) => _me.GetNextValidTargets(cardindex, already_params);
        public CostVariable GetEventFinalDiceRequirement(NetOperation action) => _me.GetEventFinalDiceRequirement(action);
        internal void BindTeam(PlayerTeam me)
        {
            _me = me;
            Game = new(me.Game, me.TeamIndex);
            BindInit();
        }
        public virtual void BindInit() { }
        public virtual void Update(ClientUpdatePacket packet) => Game.Update(packet);
        /// <summary>
        /// TODO:偷个懒
        /// </summary>
        public virtual void UpdateRegion() => Game.UpdateRegion(_me, _me.Enemy);
    }
}
