using System.Text.Json;

namespace TCGInfo
{
    /// <summary>
    /// 双方都能看到的队伍信息(没有特殊模式的前提下)
    /// </summary>
    public class TeamInfo
    {
        /// <summary>
        /// <see cref="PlayerInfo"/>的json
        /// </summary>
        public string Player { get; set; }

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
