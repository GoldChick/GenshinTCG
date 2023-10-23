using GenshinTCG.TCGMod.Util.Persistent;
using TCGBase;
using TCGCard;
using TCGGame;
using TCGMod;

namespace Genshin3_3
{
    public class 丘丘岩盔王 : AbstractCardCharacter
    {
        public override AbstractCardSkill[] Skills => new AbstractCardSkill[]
        {
            new CharacterSimpleA("a",0,2,5),
            new CharacterSimpleE("e",0,3,5),
            new CharacterSimpleQ("q",0,5,5),
            new Passive()
        };

        public override ElementCategory CharacterElement => ElementCategory.GEO;

        public override WeaponCategory WeaponCategory => WeaponCategory.OTHER;

        public override CharacterRegion CharacterRegion => CharacterRegion.QQ;
        public override CharacterCategory CharacterCategory => CharacterCategory.MOB;

        public override string NameID => "qq";
        private class Passive : AbstractPassiveSkill
        {
            public override string NameID => "检验治理";

            public override string[] TriggerDic => new string[] { Tags.SenderTags.GAME_START };

            public override bool TriggerOnce => true;

            public override void AfterUseAction(PlayerTeam me, Character c, int[]? targetArgs = null)
            {
                me.AddPersistent(new 岩盔(), c.Index);
            }
            private class 岩盔 : AbstractCardPersistentEffect
            {
                public override int MaxUseTimes => 3;

                public override Dictionary<string, PersistentTrigger> TriggerDic => new()
                {
                    {Tags.SenderTags.HURT_DECREASE,new PersistentPurpleShield(1) },
                    {Tags.SenderTags.ROUND_START,new((me,p,s,v)=>me.AddPersistent(new 坚岩之力(),p.PersistentRegion,p))}
                };

                public override string NameID => "岩盔";
            }
            private class 坚岩之力 : AbstractCardPersistentEffect
            {
                public override int MaxUseTimes => 1;

                public override Dictionary<string, PersistentTrigger> TriggerDic => new()
                {
                    { Tags.SenderTags.ELEMENT_ENCHANT,new((me,p,s,v)=>
                        {
                            if (PersistentFunc.IsCurrCharacterDamage(me,p,s,v,out var dv))
                            {
                                dv.Element=5;
                                dv.Damage++;
                                p.AvailableTimes--;
                            }
                        }
                    )}
                };

                public override string NameID => "坚岩之力";
            }
        }
    }
}
