﻿using System.Text.Json.Serialization;

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
        public ReadonlyCharacter(Character c) : base(c.CardBase.Namespace, c.CardBase.NameID)
        {
            HP = c.HP;
            MP = c.MP;
            MaxHP = c.Card.MaxHP;
            MaxMP = c.Card.MaxMP;
            Element = c.Element;
            Effects = c.Effects.Copy().Select(e => new ReadonlyPersistent(e)).ToList();
            SkillCount = c.CardBase.TriggerableList.Where(t => t.Tag == SenderTagInner.UseSkill.ToString()).Count();
        }
    }
    public class ReadonlyPersistent : ReadonlyObject
    {
        public PersistentType Type { get; }
        public int AvailableTimes { get; internal set; }
        public IEnumerable<int> Data { get; }
        [JsonConstructor]
        public ReadonlyPersistent(string nameSpace, string nameid, PersistentType type, int availabletimes, IEnumerable<int> data) : base(nameSpace, nameid)
        {
            Type = type;
            AvailableTimes = availabletimes;
            Data = data;
        }
        internal ReadonlyPersistent(Persistent p) : this(p.CardBase.Namespace, p.CardBase.NameID, GetPersistentType(p.CardBase), p.AvailableTimes, p.Data)
        {
        }
        public static PersistentType GetPersistentType(AbstractCardBase cardbase)
        {
            switch (cardbase.CardType)
            {
                case CardType.Summon:
                    return PersistentType.Summon;
                case CardType.Equipment:
                    if (cardbase.Tags.Contains(CardTag.Weapon.ToString()))
                    {
                        return PersistentType.Weapon;
                    }
                    else if (cardbase.Tags.Contains(CardTag.Artifact.ToString()))
                    {
                        return PersistentType.Artifact;
                    }
                    else if (cardbase.Tags.Contains(CardTag.Talent.ToString()))
                    {
                        return PersistentType.Talent;
                    }
                    return PersistentType.Effect;
                case CardType.Support:
                    return PersistentType.Support;
                case CardType.Effect:
                    return PersistentType.Effect;
                default:
                    throw new ArgumentException($"ReadonlyPersistent: Wrong CardType as Persistent ! Name: {cardbase.Namespace}:{cardbase.NameID}");
            }
        }
        public enum PersistentType
        {
            Effect,
            Weapon,
            Artifact,
            Talent,
            Summon,
            Support
        }
    }

}
