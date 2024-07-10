namespace TCGBase
{
    /// <summary>
    /// 专为我们lua设计
    /// </summary>
    public interface IPlayerTeamAPI
    {
        /// <summary>
        /// 向牌库里增加卡牌
        /// </summary>
        //public Persistent? InsertCard();
        /// <summary>
        /// 获得手牌
        /// </summary>
        public Persistent? AddCard(string nameid);
        /// <summary>
        /// 如果target是角色、角色状态，则增加作为新的角色状态；否则增加作为出战状态
        /// </summary>
        public Persistent? AddEffect(string nameid, Persistent? father = null, object? targetArea = null);
        public Persistent? AddSummon(string nameid, Persistent? father = null);
        /// <summary>
        /// 抽取带有标签的卡牌
        /// </summary>
        public void DrawCard(int num = 1, params string[] tags);
        /// <summary>
        /// 将场上的状态p(卡牌)回手
        /// </summary>
        public void PopTo(Persistent p, TargetType type = TargetType.Hand);
        public void GainDice(object element, int count = 1);

        /// <summary>
        /// 如果revive=false，则在目标角色被击倒时，会复苏
        /// </summary>
        public void Heal(Persistent persistent, AbstractTriggerable triggerable, int amount, int targetIndex = 0, bool targetRelative = true, bool revive = false);
        public void UseSkill(Character c, int skill_index);
        public void PrepareSkill(Character c, int prepareskill_index);
        public void Trigger(Persistent source, string senderID);

        /// <summary>
        /// 强制切换到某一个[活]角色（可指定绝对坐标或相对坐标，默认绝对）<br/>
        /// </summary>
        public void SwitchTo(int targetIndex, bool targetRelative = false);
    }
}
