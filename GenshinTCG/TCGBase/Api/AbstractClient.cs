namespace TCGBase
{
    /// <summary>
    /// 尽管名字叫Client，实际上是在服务端运行的<br/>
    /// 其实更应该是Server与Client之间交互的通道，只是想不起来应该叫什么名字了
    /// </summary>
    public abstract class AbstractClient
    {
        public ClientSetting ClientSetting { get; protected set; }
        public ServerSetting ServerSetting { get; protected set; }

        public ReadonlyGame Game { get; protected set; }
        private PlayerTeam Me { get; set; }

        /// <summary>
        /// 服务端=>客户端
        /// 客户端链接时更新Setting
        /// </summary>
        public abstract void InitServerSetting(ServerSetting setting);

        /// <summary>
        /// 客户端=>服务端
        /// 游戏开始前传入卡组
        /// </summary>
        public abstract ServerPlayerCardSet RequestCardSet();
        /// <summary>
        /// 客户端=>服务端
        /// 游戏进行中调用索取对应行动
        /// </summary>
        public abstract NetEvent RequestEvent(ActionType demand, string help_txt = "Null");
        /// <summary>
        /// 表示正在向对方request需要的event
        /// </summary>
        public virtual void RequestEnemyEvent(ActionType demand) { }
        public List<TargetEnum> GetTargetEnums(NetAction action) => Me.GetTargetEnums(action);
        public DiceCostVariable GetEventFinalDiceRequirement(NetAction action) => Me.GetEventFinalDiceRequirement(action);
        public bool IsEventValid(NetEvent evt) => Me.IsEventValid(evt);
        /// <summary>
        /// 服务端=>客户端
        /// 游戏进行中更新Team<br/>
        /// </summary>
        public void BindTeam(PlayerTeam me)
        {
            Me = me;
            Game = new(me.Game, me.TeamIndex);
            BindInit(Game);
        }
        public virtual void BindInit(ReadonlyGame game)
        {

        }
        public virtual void Update(ClientUpdatePacket packet) => Game.Update(packet);
        /// <summary>
        /// TODO:偷个懒
        /// </summary>
        public virtual void UpdateRegion() => Game.UpdateRegion(Me, Me.Enemy);
    }
}
