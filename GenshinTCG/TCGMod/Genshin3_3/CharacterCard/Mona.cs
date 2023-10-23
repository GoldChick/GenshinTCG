using TCGCard;
using TCGGame;
using TCGMod;
using TCGBase;

namespace Genshin3_3
{
    public class Mona : AbstractCardCharacter
    {
        public override int MaxMP => 3;

        public override AbstractCardSkill[] Skills => new AbstractCardSkill[] {
            new CharacterSimpleA("因果点破",2,1),
            new CharacterSingleSummonE("水中幻愿",2,1,new 虚影()),
            new 星命定轨()};

        public override string NameID => "mona";

        public override ElementCategory CharacterElement => ElementCategory.HYDRO;

        public override WeaponCategory WeaponCategory => WeaponCategory.CATALYST;

        public override CharacterRegion CharacterRegion => CharacterRegion.MONDSTADT;

        private class 虚影 : AbstractCardPersistentSummon
        {
            public override bool DeleteWhenUsedUp => false;
            public override int MaxUseTimes => 1;
            public override Dictionary<string, PersistentTrigger> TriggerDic => new() {
                { TCGBase.Tags.SenderTags.HURT_DECREASE, new PersistentPurpleShield(1) },
                { TCGBase.Tags.SenderTags.ROUND_OVER, new ((me,p,s,v)=>
                {
                    p.Active = false;
                    me.Enemy.Hurt(new(2, 1,  0),this);
                })}
            };

            public override string NameID => "虚影";
        }
        private class 星命定轨 : AbstractCardSkill
        {
            public override int[] Costs => new int[] { 0, 0, 3 };
            public override string NameID => "星命定轨";
            public override SkillCategory Category => SkillCategory.Q;

            public override void AfterUseAction(PlayerTeam me, Character c, int[]? targetArgs = null)
            {
                me.Enemy.Hurt(new DamageVariable(2, 4, 0), this);
                me.AddPersistent(new 泡影());
            }
            private class 泡影 : AbstractCardPersistentEffect
            {
                public override int MaxUseTimes => 1;

                public override Dictionary<string, PersistentTrigger> TriggerDic => new() {
                    { Tags.SenderTags.HURT_MUL,new 泡影_Trigger()} };

                public override string NameID => "泡影";
                private class 泡影_Trigger : PersistentTrigger
                {
                    public void Trigger(PlayerTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
                    {
                        if (variable is DamageVariable dv)
                        {
                            dv.Damage *= 2;
                            persitent.Active = false;
                        }
                    }
                }
            }
        }
    }
}
