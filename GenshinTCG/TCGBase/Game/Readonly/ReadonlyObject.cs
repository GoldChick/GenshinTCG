using System.Text.Json.Serialization;

namespace TCGBase
{
    public class ReadonlyObject
    {
        public string NameSpace { get; init; }
        public string NameID { get; init; }
        [JsonConstructor]
        public ReadonlyObject(string nameSpace, string name)
        {
            NameSpace = nameSpace;
            NameID = name;
        }
    }
    public class ReadonlyCharacter : ReadonlyObject
    {
        private int _hp;
        public int HP { get => _hp; set => _hp = int.Max(0, value); }
        public int MP { get; set; }
        public int MaxHP { get; init; }
        public int MaxMP { get; init; }
        public int Element { get; set; }
        public List<ReadonlyPersistent> Effects { get; }
        public int SkillCount { get; }
        [JsonConstructor]
        public ReadonlyCharacter(string nameSpace, string name, int hP, int mP, int maxHP, int maxMP, int element, List<ReadonlyPersistent> effects, int skillCount) : base(nameSpace, name)
        {
            HP = hP;
            MP = mP;
            MaxHP = maxHP;
            MaxMP = maxMP;
            Element = element;
            Effects = effects;
            SkillCount = skillCount;
        }
        public ReadonlyCharacter(Character c) : base(c.Card.Namespace, c.Card.NameID)
        {
            HP = c.HP;
            MP = c.MP;
            MaxHP = c.Card.MaxHP;
            MaxMP = c.Card.MaxMP;
            Element = c.Element;
            Effects = c.Effects.Copy().Select(e => new ReadonlyPersistent(e)).ToList();
            SkillCount = c.Card.Skills.Where(s => s.Category != SkillCategory.P).Count();
        }
    }
    public class ReadonlyPersistent : ReadonlyObject
    {
        public int[] Infos { get; set; }
        [JsonConstructor]
        public ReadonlyPersistent(string nameSpace, string name, params int[] infos) : base(nameSpace, name)
        {
            Infos = infos;
        }
        internal ReadonlyPersistent(AbstractPersistent p) : this(p.CardBase.TextureNameSpace, p.CardBase.TextureNameID, p.CardBase.Info(p))
        {
        }
    }

}
