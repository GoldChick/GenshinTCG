namespace TCGInfo
{
    /// <summary>
    /// 其中的类全部用于序列化与反序列化
    /// 只传递需要在客户端显示的内容
    /// </summary>
    public struct EffectInfo
    {
        public string EffectID { get; set; }
        public int LeftTimes { get; set; }
    }

    /// <summary>
    /// 在场角色的全部信息
    /// </summary>
    public struct CharacterInfo
    {
        public string CharacterID { get; }
        public int MaxMp { get; }
        public int Hp { get; set; }
        public int Mp { get; set; }
        public string Weapon { get; set; }
        public string Artifact { get; set; }
        public string Nature { get; set; }
        public string Tags { get; }
        /// <summary>
        /// 若干个EffectInfo
        /// </summary>
        public string[] Effects { get; set; }
        public string[] Skills { get; set; }
        /// <summary>
        /// 角色技能的Info
        /// </summary>
        public struct SkillInfo
        {
            public string SkillID { get; }
            public bool SameDice { get; }
            public byte DiceType { get; }
            /// <summary>
            /// 经过buff计算后的骰子需求
            /// </summary>
            public int[] Dices { get; set; }
        }
    }
    /// <summary>
    /// 双方都能看到的队伍信息(没有特殊模式的前提下)
    /// </summary>
    public class TeamInfo
    {
        /// <summary>
        /// 玩家的头像，只能使用指定内容
        /// </summary>
        public string Avatar { get; }
        /// <summary>
        /// 玩家的昵称
        /// </summary>
        public string Name { get; }

        public int CurrCharacter { get; set; }
        /// <summary>
        ///  里面是3个<see cref="CharacterInfo"/>的json
        /// </summary>
        public string[] Characters { get; set; }
        /// <summary>
        /// 团队效果，里面是若干<see cref="EffectInfo"/>的json
        /// </summary>
        public string[] TeamEffects { get; set; }
        /// <summary>
        /// 已经打出的辅助牌
        /// </summary>
        public string[] AssistEffects { get; set; }
        /// <summary>
        /// 在场的召唤物
        /// </summary>
        public string[] SummonEffects { get; set; }

        public int DiceNum { get; set; }
        public int CardsNum { get; set; }
        public int LeftCardsNum { get; set; }
    }
    /// <summary>
    /// 只有己方能看到的队伍信息
    /// </summary>
    public class TeamInfoDetail : TeamInfo
    {
        public string[] Cards { get; set; }
        public string[] LeftCards { get; set; }
        public int[] Dices { get; set; }
    }
}
