namespace TCGBase
{
    public record BaseGameRecord
    {
        public string NameID { get; }
        public bool Hidden { get; }

        protected BaseGameRecord(string nameID, bool hidden)
        {
            NameID = nameID;
            Hidden = hidden;
        }
    }
    public record BaseCardRecord : BaseGameRecord
    {
        /// <summary>
        /// 卡牌一共分两大类别：角色牌，行动牌<br/>
        /// 行动牌又有五大类别：装备牌，支援牌，事件牌，状态牌，召唤物牌
        /// </summary>
        public CardType CardType { get; }
        public BaseCardRecord(string nameID, bool hidden, CardType cardType, List<string> skillList, List<string> tags) : base(nameID, hidden)
        {
            CardType = cardType;
            SkillList = skillList;
            Tags = tags;
        }
        public List<string> SkillList { get; }
        public List<string> Tags { get; }
    }
    public record ActionCardRecord : BaseCardRecord
    {
        public ActionCardRecord(string nameID, bool hidden, CardType cardType, List<string> skillList, List<string> tags, int maxNumPermitted = 2) : base(nameID, hidden, cardType, skillList, tags)
        {
            MaxNumPermitted = maxNumPermitted;
        }
        public int MaxNumPermitted { get; }
        //TODO: choose condition & select condition
    }
    public record CharacterCardRecord : BaseCardRecord
    {
        public CharacterCardRecord(string nameID, int maxHP, int maxMP, ElementCategory characterElement, List<string> skillList, List<string> tags, bool hidden = false, CardType cardType = CardType.Character) : base(nameID, hidden, cardType, skillList, tags)
        {
            MaxHP = maxHP;
            MaxMP = maxMP;
            CharacterElement = characterElement;
        }

        public int MaxHP { get; }
        public int MaxMP { get; }
        public ElementCategory CharacterElement { get; }
    }
}
