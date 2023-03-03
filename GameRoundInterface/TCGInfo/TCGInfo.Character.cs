namespace TCGInfo
{
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
}
