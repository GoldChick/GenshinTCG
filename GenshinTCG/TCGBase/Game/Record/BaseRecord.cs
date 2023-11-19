namespace TCGBase
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
        public bool FightAction { get; }
        public NetEventRecord(int teamid, NetEvent netEvent, bool fightaction) : base(teamid)
        {
            NetEvent = netEvent;
            FightAction = fightaction;
        }
    }
    public record UseSkillRecord : NetEventRecord
    {
        public Character Character { get; }
        public AbstractCardSkill Skill { get; }

        public UseSkillRecord(int teamid, NetEvent netEvent, bool fightAction, Character character, AbstractCardSkill skill) : base(teamid, netEvent, fightAction)
        {
            Character = character;
            Skill = skill;
        }
    }
    public record UseCardRecord : NetEventRecord
    {
        public AbstractCardAction Card { get; }
        public UseCardRecord(int teamid, NetEvent netEvent, bool fightAction, AbstractCardAction card) : base(teamid, netEvent, fightAction)
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
