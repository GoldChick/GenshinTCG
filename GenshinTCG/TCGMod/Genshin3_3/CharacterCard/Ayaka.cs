using TCGBase;
using TCGCard;
using TCGGame;
using TCGMod;

namespace Genshin3_3
{
    public class Ayaka : AbstractCardCharacter
    {
        //TODO:for test
        public override int MaxMP => 1;
        public override AbstractCardSkill[] Skills => new AbstractCardSkill[] {
            new CharacterSimpleA("a",0,2,1),
            new CharacterSimpleE("e",1,3),
            new CharacterSingleSummonQ("q",1,4,new SimpleSummon("双肩血管飞",1,2,2)),
            new 神里流霰步(),
        };

        public override ElementCategory CharacterElement => ElementCategory.CRYO;

        public override WeaponCategory WeaponCategory => WeaponCategory.SWORD;

        public override CharacterRegion CharacterRegion => CharacterRegion.INAZUMA;

        public override string NameID => "ayaka";
        private class 神里流霰步 : AbstractPassiveSkill
        {
            public override string[] TriggerDic => new string[] { Tags.SenderTags.AFTER_SWITCH };

            public override bool TriggerOnce => false;

            public override string NameID => "神里流霰步";

            public override void AfterUseAction(PlayerTeam me, Character c, int[]? targetArgs = null)
            {
                if (targetArgs[0] == me.TeamIndex && me.CurrCharacter == c.Index)
                {
                    me.AddPersistent(new Enchant("冰附魔", 1, 1), c.Index);
                }
            }
        }
    }
}
