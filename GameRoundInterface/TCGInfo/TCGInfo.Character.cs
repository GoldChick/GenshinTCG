namespace TCGInfo
{
    /// <summary>
    /// 在场角色的可变信息
    /// </summary>
    public class CharacterInfo
    {
        public CharacterInfo(int hp, int mp, string weapon, string artifact, string nature, string[] effects, string[] skills)
        {
            Hp = hp;
            Mp = mp;
            Weapon = weapon;
            Artifact = artifact;
            Nature = nature;
            Effects = effects;
            Skills = skills;
        }

        public int Hp { get; set; }
        public int Mp { get; set; }
        public string Weapon { get; set; }
        public string Artifact { get; set; }
        public string Nature { get; set; }
        /// <summary>
        /// 若干个EffectInfo
        /// </summary>
        public string[] Effects { get; set; }
        /// <summary>
        /// 可能受到减费等影响的技能
        /// </summary>
        public string[] Skills { get; set; }
    }
    /// <summary>
    /// 角色技能的Info
    /// </summary>
    public class SkillInfo
    {
        public string SkillID { get; }
        public bool SameDice { get; }
        public byte DiceType { get; }
        /// <summary>
        /// 经过buff计算后的骰子需求
        /// </summary>
        public int[] Dices { get; set; }
        public SkillInfo(string skillID, bool sameDice, byte diceType, int[] dices)
        {
            SkillID = skillID;
            SameDice = sameDice;
            DiceType = diceType;
            Dices = dices;
        }
    }

}
