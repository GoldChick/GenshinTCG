﻿namespace TCGBase
{
    /// <summary>
    /// 不携带任何参数，只有name的sender
    /// </summary>
    public class OnCharacterOnSender : SimpleSender, IPeristentSupplier
    {
        public Character Character { get; }
        public bool Start { get; }
        Persistent IPeristentSupplier.Persistent => Character;
        public OnCharacterOnSender(int teamID, Character character, bool start = false) : base(teamID)
        {
            Character = character;
            Start = start;
        }
    }
}
