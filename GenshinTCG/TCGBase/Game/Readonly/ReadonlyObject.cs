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
        public List<string> Skills { get; }
        public ReadonlyCharacter(Character c) : base(c.Card.NameID)
        {
            HP = c.HP;
            MP = c.MP;
            MaxHP = c.Card.MaxHP;
            MaxMP = c.Card.MaxMP;
            Element = c.Element;
            Effects = c.Effects.Copy().Select(e => new ReadonlyPersistent(e.NameID, e.Card.Info(e))).ToList();
            var temp = c.Weapon;
            if (temp != null)
            {
                Weapon = new(temp.NameID, temp.Card.Info(temp));
            }
            temp = c.Artifact;
            if (temp != null)
            {
                Artifact = new(temp.NameID, temp.Card.Info(temp));
            }
            var tempt = c.Talent;
            if (tempt != null)
            {
                Weapon = new(tempt.NameID, tempt.Card.Info(tempt));
            }
            Skills = c.Card.Skills.Select(p => p.NameID).ToList();
        }
    }
    public class ReadonlyPersistent : AbstractReadonlyObject
    {
        public int[] Infos { get; set; }
        public ReadonlyPersistent(string name, params int[] infos) : base(name)
        {
            Infos = infos;
        }
    }

}
