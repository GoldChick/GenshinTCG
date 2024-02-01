namespace TCGBase
{
    public record BaseGameRecord
    {
        public string Name { get; }
        public string Description { get; }
        public bool Hidden { get; }

        protected BaseGameRecord(string name, string description, bool hidden)
        {
            Name = name;
            Description = description;
            Hidden = hidden;
        }
    }
    public record SkillRecord : BaseGameRecord
    {
        protected SkillRecord(BaseGameRecord original) : base(original)
        {
        }
    }
    public record BaseCardRecord : BaseGameRecord
    {
        public BaseCardRecord(string name, string description, bool hidden, List<string> skillList, List<string> tags) : base(name, description, hidden)
        {
            SkillList = skillList;
            Tags = tags;
        }

        public List<string> SkillList { get; }
        public List<string> Tags { get; }
    }
    public record CharacterCardRecord : BaseCardRecord
    {
        public CharacterCardRecord(string name, string description, bool hidden, List<string> skillList, List<string> tags, int maxHP, int maxMP) : base(name, description, hidden, skillList, tags)
        {
            MaxHP = maxHP;
            MaxMP = maxMP;
        }

        public int MaxHP { get; }
        public int MaxMP { get; }
    }
}
