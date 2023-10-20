using TCGBase;
using TCGCard;
using TCGGame;
using TCGMod;

namespace Genshin3_3
{
    public class XiangLing : AbstractCardCharacter
    {
        public override AbstractCardSkill[] Skills => new AbstractCardSkill[] {
        new CharacterSimpleA("白案功夫",0,2,3),
        new CharacterSingleSummonE("锅巴出击",new 锅巴(),3),
        new 旋火轮()};

        public override string NameID => "xiangling";

        public override ElementCategory CharacterElement => ElementCategory.PYRO;

        public override WeaponCategory WeaponCategory => WeaponCategory.LONGWEAPON;

        public override CharacterRegion CharacterRegion => CharacterRegion.LIYUE;

        private class 旋火轮 : AbstractCardSkill
        {
            //TODO: free temp
            public override int[] Costs => new int[] { };

            public override bool CostSame => true;

            public override string NameID => "旋火轮";

            public override SkillCategory Category => SkillCategory.Q;

            public override void AfterUseAction(AbstractTeam me, Character c, int[]? targetArgs = null)
            {
                me.Enemy.Hurt(new DamageVariable(3, 3, 0), this);
                me.AddPersistent(new 火轮());
            }
            private class 火轮 : AbstractCardPersistentEffect
            {
                public override int MaxUseTimes => 2;

                public override Dictionary<string, PersistentTrigger> TriggerDic => new()
                {
                    { Tags.SenderTags.AFTER_USE_SKILL,new((me,p,s,v)=>
                    {
                        if (s is UseSkillSender ski && ski.Skill is not 旋火轮)
                        {
                            me.Enemy.Hurt(new DamageVariable(3, 2, 0), this);
                            p.AvailableTimes--;
                        }
                    }
                    )}
                };


                public override string NameID => "火轮";
            }
        }
    }
    public class 锅巴 : AbstractCardPersistentSummon
    {
        public override int MaxUseTimes => 2;

        public override Dictionary<string, PersistentTrigger> TriggerDic => new() {
            { Tags.SenderTags.ROUND_OVER,new((me,p,s,v)=>
                {
                    me.Enemy.Hurt(new(3, 2, 0),this);
                    p.AvailableTimes--;
                }
            )}};

        public override string NameID => "guoba";
    }
}
