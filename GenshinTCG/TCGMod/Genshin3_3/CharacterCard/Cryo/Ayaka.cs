using TCGBase;
 

namespace Genshin3_3
{
    public class Ayaka : AbstractCardCharacter
    {
        public override int MaxMP => 3;
        public override AbstractCardSkill[] Skills => new AbstractCardSkill[] {
            new CharacterSimpleA("a",0,2,1),
            new CharacterSimpleE("e",1,3),
            new CharacterSingleSummonQ("q",1,4,new SimpleSummon("双肩血管飞",1,2,2)),
            new 神里流霰步(),
        };

        public override ElementCategory CharacterElement => ElementCategory.Cryo;

        public override WeaponCategory WeaponCategory => WeaponCategory.SWORD;

        public override CharacterRegion CharacterRegion => CharacterRegion.INAZUMA;

        public override string NameID => "ayaka";
        private class 神里流霰步 : AbstractPassiveSkill
        {
            public override string[] TriggerDic => new string[] { SenderTag.AfterSwitch.ToString() };

            public override bool TriggerOnce => false;

            public override string NameID => "神里流霰步";

            public override void AfterUseAction(PlayerTeam me, Character c, int[]? targetArgs = null)
            {
                if (targetArgs[0] == me.TeamIndex && me.CurrCharacter == c.Index)
                {
                    if (c.Talent!=null &&c.Talent.NameID== "寒天宣命祝词")
                    {
                        Console.WriteLine("寒天触发了？？");
                    }
                    me.AddPersistent(new Enchant("冰附魔", 1, 1), c.Index);
                }
            }
        }
    }
    public class 寒天宣命祝词 : AbstractCardTalent
    {
        private static readonly Ayaka _ayaka = new();
        public override string CharacterNameID => _ayaka.NameID;

        public override int[] Costs => new int[] { 2 };

        public override string NameID => "寒天宣命祝词";

        public override AbstractCardPersistentTalent Effect => new 天赋();

        public override int Skill => 3;
        public class 天赋 : AbstractCardPersistentTalent
        {
            public override int MaxUseTimes => 1;
            public override int Skill => 3;
            public override string NameID => "寒天宣命祝词";
        }
    }
}
