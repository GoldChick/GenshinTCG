using System.Text.Json.Serialization;

namespace TCGBase
{
    public class ReadonlyObject
    {
        public string NameSpace { get; init; }
        public string NameID { get; init; }
        [JsonConstructor]
        public ReadonlyObject(string nameSpace, string nameid)
        {
            NameSpace = nameSpace;
            NameID = nameid;
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
        public ReadonlyCharacter(string nameSpace, string nameid, int hP, int mP, int maxHP, int maxMP, int element, List<ReadonlyPersistent> effects, int skillCount) : base(nameSpace, nameid)
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
            SkillCount = c.Card.TriggerableList.Where(t => t.Tag == SenderTagInner.UseSkill.ToString()).Count();
        }
    }
    public class ReadonlyPersistent : ReadonlyObject
    {
        public int Variant { get; }
        public int AvailableTimes { get; internal set; }
        [JsonConstructor]
        public ReadonlyPersistent(string nameSpace, string nameid, int variant, int availabletimes) : base(nameSpace, nameid)
        {
            Variant = variant;
            AvailableTimes = availabletimes;
        }
        internal ReadonlyPersistent(AbstractPersistent p) : this("sb", p.CardBase.NameID, p.CardBase.Variant, p.AvailableTimes)
        {
            //TODO: p.CardBase.Namespace TODO23
        }
    }

}
