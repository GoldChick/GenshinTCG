namespace TCGBase
{
    /// <summary>
    /// 尽管名字叫Client，实际上是在服务端运行的<br/>
    /// 其实更应该是Server与Client之间交互的通道，只是想不起来应该叫什么名字了
    /// </summary>
    public abstract class AbstractClient
    {
        public ReadonlyGame Game { get; protected set; }
        private PlayerTeam Me { get; set; }

        /// <summary>
        /// 客户端=>服务端
        /// 游戏开始前传入卡组
        /// </summary>
        public abstract ServerPlayerCardSet RequestCardSet();
        /// <summary>
        /// 客户端=>服务端
        /// 游戏进行中调用索取对应行动
        /// </summary>
        public abstract NetEvent RequestEvent(ActionType demand);
        /// <summary>
        /// 表示正在向对方request需要的event
        /// </summary>
        public virtual void RequestEnemyEvent(ActionType demand) { }
        public List<TargetEnum> GetCardTargetEnums(int cardindex) => Me.GetCardTargetEnums(cardindex);
        public List<int> GetCardNextValidTargets(int cardindex, int[] already_params) => Me.GetNextValidTargets(cardindex, already_params);
        public DiceCostVariable GetEventFinalDiceRequirement(NetAction action) => Me.GetEventFinalDiceRequirement(action);
        public (DiceCostVariable, int) GetCardCostRequirement(int index) => (Me.GetEventFinalDiceRequirement(new(ActionType.UseCard, index)), Me.CardsInHand[index] is IEnergyConsumer ec ? ec.CostMP : 0);
        public (DiceCostVariable, int) GetSkillCostRequirement(int index) => (Me.GetEventFinalDiceRequirement(new(ActionType.UseSKill, index)), Me.Characters[Me.CurrCharacter].Card is AbstractCardCharacter c && c.Skills[index].Category == SkillCategory.Q ? c.MaxMP : 0);
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
