﻿using System.Text.Json.Serialization;

namespace TCGClient
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
        [JsonConstructor]
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
            /// int[0]: index<br/>int[1]: element<br/>int[2]: damage
            /// </summary>
            Hurt,
            /// <summary>
            /// int[0]: index<br/>int[1]: amount
            /// </summary>
            Heal,
            /// <summary>
            /// int[0]: index<br/>int[1]: element
            /// </summary>
            AttachElement,
            /// <summary>
            /// int[0]: index
            /// </summary>
            Die,
            /// <summary>
            /// int[0]: index<br/>int[1]: skill index
            /// </summary>
            UseSkill,
            PreSwitch,
            /// <summary>
            /// int[0]: index<br/>int[1]: is forced ()1=true
            /// </summary>
            Switch
        }
        public static class CharacterUpdate
        {
            public static ClientUpdatePacket CharacterHurtUpdate(int teamID, int index, int element, int damage) => new(ClientUpdateType.Character, 10 * teamID + (int)CharacterUpdateCategory.Hurt, index, element, damage);
            public static ClientUpdatePacket CharacterHealUpdate(int teamID, int index, int amount) => new(ClientUpdateType.Character, 10 * teamID + (int)CharacterUpdateCategory.Heal, index, amount);
            public static ClientUpdatePacket CharacterElementUpdate(int teamID, int index, int element) => new(ClientUpdateType.Character, 10 * teamID + (int)CharacterUpdateCategory.AttachElement, index, element);
            public static ClientUpdatePacket CharacterDieUpdate(int teamID, int index) => new(ClientUpdateType.Character, 10 * teamID + (int)CharacterUpdateCategory.Die, index);
            public static ClientUpdatePacket CharacterUseSkillUpdate(int teamID, int index, int skillIndex) => new(ClientUpdateType.Character, 10 * teamID + (int)CharacterUpdateCategory.UseSkill, index, skillIndex);
            public static ClientUpdatePacket CharacterPreSwitchUpdate(int teamID) => new(ClientUpdateType.Character, 10 * teamID + (int)CharacterUpdateCategory.PreSwitch);
            public static ClientUpdatePacket CharacterSwitchUpdate(int teamID, int index, bool isForced) => new(ClientUpdateType.Character, 10 * teamID + (int)CharacterUpdateCategory.Switch, index, isForced ? 1 : 0);

        }
        public enum PersistentUpdateCategory
        {
            Act,
            Obtain,
            Lose
        }
        /// <param name="pos">persistent position (0-6 character 7 team 8 summon 9 support)</param>
        public static ClientUpdatePacket PersistentUpdate(int teamID, PersistentUpdateCategory category, int pos, string cardID) => new(ClientUpdateType.Persistent, 10 * teamID + (int)category, new int[] { pos }, new string[] { cardID });
        public enum DiceUpdateCategory
        {
            Consume,
            Obtain
        }
        public static ClientUpdatePacket DiceUpdate(int teamID, DiceUpdateCategory category, params int[] dices) => new(ClientUpdateType.Dice, 10 * teamID + (int)category, dices);
        public enum CardUpdateCategory
        {
            Use,
            Consume,
            Obtain,
            Push,
            Pop
        }
        public static ClientUpdatePacket CardUpdate(int teamID, CardUpdateCategory category, params string[] cardID) => new(ClientUpdateType.Card, 10 * teamID + (int)category, cardID);
    }
}