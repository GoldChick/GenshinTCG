namespace TCGBase
{
    public enum ClientUpdateType
    {
        None,
        /// <summary>
        /// int[0]: after
        /// </summary>
        CurrTeam,
        /// <summary>
        /// int[0]: time
        /// </summary>
        WaitingTime,
        /// <summary>
        /// complex...
        /// see at <see cref="ClientUpdateCreate.CardUpdateCategory"/>
        /// </summary>
        Character,
        /// <summary>
        /// int[0]: persistent position (0-6 character 7 team 8 summon 9 support)
        /// string[0]: name
        /// </summary>
        Persistent,
        /// <summary>
        /// int[x]: element dice (0-7)
        /// </summary>
        Dice,
        /// <summary>
        /// string[x]: card name
        /// </summary>
        Card
    }
    public class ClientUpdatePacket
    {
        public ClientUpdateType Type { get; }
        /// <summary>
        /// 十位表示队伍(0 or 1)<br/>
        /// 个位表示具体的category
        /// </summary>
        public int Category { get; }
        public int[] Ints { get; }
        public string[] Strings { get; }
        /// <summary>
        /// 仅为json使用
        /// </summary>
        public ClientUpdatePacket(ClientUpdateType type, int category, int[] ints, string[] strings)
        {
            Type = type;
            Category = category;
            Ints = ints;
            Strings = strings;
        }
        internal ClientUpdatePacket(ClientUpdateType type, int category, params int[] ints)
        {
            Type = type;
            Category = category;
            Ints = ints;
            Strings = Array.Empty<string>();
        }
        internal ClientUpdatePacket(ClientUpdateType type, int category, params string[] strings)
        {
            Type = type;
            Category = category;
            Ints = Array.Empty<int>();
            Strings = strings;
        }
        internal ClientUpdatePacket(ClientUpdateType type, int category)
        {
            Type = type;
            Category = category;
            Ints = Array.Empty<int>();
            Strings = Array.Empty<string>();
        }
    }
    internal static class ClientUpdateCreate
    {
        public static ClientUpdatePacket CurrTeamUpdate(int after) => new(ClientUpdateType.CurrTeam, 0, after);
        public static ClientUpdatePacket WaitingTimeUpdate(int time) => new(ClientUpdateType.WaitingTime, 0, time);
        public enum CharacterUpdateCategory
        {
            /// <summary>
            /// int[0]: char index<br/>int[1]: element<br/>int[2]: damage
            /// </summary>
            Hurt,
            /// <summary>
            /// int[0]: char index<br/>int[1]: amount
            /// </summary>
            Heal,
            /// <summary>
            /// int[0]: char index<br/>int[1]: element
            /// </summary>
            ChangeElement,
            /// <summary>
            /// int[0]: index
            /// </summary>
            Die,
            /// <summary>
            /// int[0]: index<br/>int[1]: skill index
            /// </summary>
            UseSkill,
            /// <summary>
            /// int[0]: target index
            /// </summary>
            Switch
        }
        public static class CharacterUpdate
        {
            public static ClientUpdatePacket HurtUpdate(int teamID, int index, int element, int damage) => new(ClientUpdateType.Character, 10 * teamID + (int)CharacterUpdateCategory.Hurt, index, element, damage);
            public static ClientUpdatePacket HealUpdate(int teamID, int index, int amount) => new(ClientUpdateType.Character, 10 * teamID + (int)CharacterUpdateCategory.Heal, index, amount);
            public static ClientUpdatePacket ElementUpdate(int teamID, int index, int element) => new(ClientUpdateType.Character, 10 * teamID + (int)CharacterUpdateCategory.ChangeElement, index, element);
            public static ClientUpdatePacket MPUpdate(int teamID, int index, int mp) => new(ClientUpdateType.Character, 10 * teamID + (int)CharacterUpdateCategory.ChangeElement, index, mp);
            public static ClientUpdatePacket DieUpdate(int teamID, int index) => new(ClientUpdateType.Character, 10 * teamID + (int)CharacterUpdateCategory.Die, index);
            public static ClientUpdatePacket UseSkillUpdate(int teamID, int index, int skillIndex) => new(ClientUpdateType.Character, 10 * teamID + (int)CharacterUpdateCategory.UseSkill, index, skillIndex);
            public static ClientUpdatePacket SwitchUpdate(int teamID, int target) => new(ClientUpdateType.Character, 10 * teamID + (int)CharacterUpdateCategory.Switch, target);

        }
        /// <summary>
        /// int[0]: persistent position (0-9 character -1 team 11 summon 12 support)
        /// </summary>
        public enum PersistentUpdateCategory
        {
            /// <summary>
            /// int[1] variant int[2] availabletimes <br/>
            /// str[0]:str[1] cardnamespace+nameid
            /// </summary>
            Obtain,
            /// <summary>
            /// int[1] index;int[2] availabletimes
            /// </summary>
            Trigger,
            /// <summary>
            /// int[1] index
            /// </summary>
            Lose
        }
        public static class PersistentUpdate
        {
            public static ClientUpdatePacket ObtainUpdate(int teamID, int region, int variant, int availabletimes, string cardNameSpace, string cardNameID) => new(ClientUpdateType.Persistent, 10 * teamID + (int)PersistentUpdateCategory.Obtain, new int[] { region, variant, availabletimes }, new string[] { cardNameID, cardNameSpace });
            public static ClientUpdatePacket TriggerUpdate(int teamID, int region, int index, int availabletimes) => new(ClientUpdateType.Persistent, 10 * teamID + (int)PersistentUpdateCategory.Trigger, region, index, availabletimes);
            public static ClientUpdatePacket LoseUpdate(int teamID, int region, int index) => new(ClientUpdateType.Persistent, 10 * teamID + (int)PersistentUpdateCategory.Lose, region, index);
        }
        public static ClientUpdatePacket DiceUpdate(int teamID, params int[] dices) => new(ClientUpdateType.Dice, 10 * teamID, dices);
        public enum CardUpdateCategory
        {
            /// <summary>
            /// 使用掉int[0]号牌（双方都能看到）
            /// </summary>
            Use,
            /// <summary>
            /// 调和掉int[0]号牌（只有自己能看到是什么牌）
            /// </summary>
            Blend,
            /// <summary>
            /// 凭空得到(string[0]:string[1])牌（只有自己能看到是什么牌,str[0]为namespace str[1]为nameid）
            /// </summary>
            Obtain,
            /// <summary>
            /// LeftCards中卡牌数量增加int[].length，CardsInHand减少<br/>
            /// int[]为从小到大的index
            /// </summary>
            Push,
            /// <summary>
            /// LeftCards中卡牌数量减少1
            /// </summary>
            Pop,
            /// <summary>
            /// 爆牌(string[0]:string[1])(只有自己能看到) 
            /// </summary>
            Broke
        }
        public static ClientUpdatePacket CardUpdate(int teamID, CardUpdateCategory category) => new(ClientUpdateType.Card, 10 * teamID + (int)category);
        /// <summary>
        /// for Blend and Use
        /// </summary>
        public static ClientUpdatePacket CardUpdate(int teamID, CardUpdateCategory category, params int[] indexs) => new(ClientUpdateType.Card, 10 * teamID + (int)category, indexs);
        /// <summary>
        /// for Obtain and Broke
        /// </summary>
        public static ClientUpdatePacket CardUpdate(int teamID, CardUpdateCategory category, params string[] cardIDs) => new(ClientUpdateType.Card, 10 * teamID + (int)category, cardIDs);
    }
}
