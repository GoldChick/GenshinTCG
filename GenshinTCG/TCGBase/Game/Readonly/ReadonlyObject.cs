namespace TCGBase
{
    public abstract class AbstractReadonlyObject
    {
        public string Name { get; }
        public AbstractReadonlyObject(string name)
        {
            Name = name;
        }
    }
    public class ReadonlyCharacter : AbstractReadonlyObject
    {
        public int HP { get; set; }
        public int MP { get; set; }
        public int MaxHP { get; init; }
        public int MaxMP { get; init; }
        public int Element { get; set; }
        public ReadonlyPersistent? Weapon { get; set; }
        public ReadonlyPersistent? Artifact { get; set; }
        public ReadonlyPersistent? Talent { get; set; }
        public List<ReadonlyPersistent> Effects { get; }
        public int SkillCount { get; }
        public ReadonlyCharacter(Character c) : base(c.Card.NameID)
        {
            HP = c.HP;
            MP = c.MP;
            MaxHP = c.Card.MaxHP;
            MaxMP = c.Card.MaxMP;
            Element = c.Element;
            Effects = c.Effects.Copy().Select(e => new ReadonlyPersistent(e.Card.TextureNameSpace, e.Card.TextureNameID, e.Card.Info(e))).ToList();
            Weapon = c.Weapon.Copy().Select(e => new ReadonlyPersistent(e.Card.TextureNameSpace, e.Card.TextureNameID, e.Card.Info(e))).ElementAtOrDefault(0);
            Artifact = c.Artifact.Copy().Select(e => new ReadonlyPersistent(e.Card.TextureNameSpace, e.Card.TextureNameID, e.Card.Info(e))).ElementAtOrDefault(0);
            Talent = c.Talent.Copy().Select(e => new ReadonlyPersistent(e.Card.TextureNameSpace, e.Card.TextureNameID, e.Card.Info(e))).ElementAtOrDefault(0);
            SkillCount = c.Card.Skills.Length;
        }
    }
    public class ReadonlyPersistent : AbstractReadonlyObject
    {
        public string NameSpace { get; init; }
        public int[] Infos { get; set; }
        public ReadonlyPersistent(string nameSpace, string name, params int[] infos) : base(name)
        {
            NameSpace = nameSpace;
            Infos = infos;
        }
    }

}
