﻿namespace TCGBase
{
    public record BaseRecord
    {
        public int TeamID { get; }
        public BaseRecord(int teamid)
        {
            TeamID = teamid;
        }
    }
    public record NetEventRecord : BaseRecord
    {
        public NetEvent NetEvent { get; }
        public NetEventRecord(int teamid, NetEvent netEvent) : base(teamid)
        {
            NetEvent = netEvent;
        }
    }
    public record UseSkillRecord : NetEventRecord
    {
        public Character Character { get; }
        public AbstractTriggerable Skill { get; }

        public UseSkillRecord(int teamid, NetEvent netEvent, Character character, AbstractTriggerable skill) : base(teamid, netEvent)
        {
            Character = character;
            Skill = skill;
        }
    }
    public record UseCardRecord : NetEventRecord
    {
        public AbstractCardAction Card { get; }
        public UseCardRecord(int teamid, NetEvent netEvent, AbstractCardAction card) : base(teamid, netEvent)
        {
            Card = card;
        }
    }
    public record DieRecord : BaseRecord
    {
        public Character Character { get; }

        public DieRecord(int teamid, Character character) : base(teamid)
        {
            Character = character;
        }
    }
}
