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
    /// <summary>
    /// 虽然名字叫[技能]，但其实叫[触发]更合适
    /// </summary>
    public record SkillRecord : BaseGameRecord
    {
        protected SkillRecord(BaseGameRecord original) : base(original)
        {
        }
    }
    public record DamageRecord
    {
        public DamageElement Element { get; }
        public int Damage { get; }
        public int TargetIndexOffset { get; }
        public DamageTargetArea TargetArea { get; }
        public DamageTargetTeam TargetTeam { get; }
        public DamageRecord? SubDamage { get; internal set; }
        public DamageRecord(DamageElement element, int damage, int targetIndexOffset = 0, DamageTargetArea targetArea = DamageTargetArea.TargetOnly, DamageTargetTeam targetTeam = DamageTargetTeam.Enemy, DamageRecord? subDamage = null)
        {
            Element = element;
            Damage = damage;
            TargetIndexOffset = targetIndexOffset;
            TargetArea = targetArea;
            TargetTeam = targetTeam;
            SubDamage = subDamage;
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
