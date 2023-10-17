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
            new CharacterTrivalNormalAttack("因果点破",2,1),
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
                { TCGBase.Tags.SenderTags.HURT_ADD, new PersistentPurpleShield(1, 1) },
                { TCGBase.Tags.SenderTags.ROUND_OVER, new ((me,p,s,v)=>
                {
                    p.Active = false;
                    me.Enemy.Hurt(new(2, 1, DamageSource.Summon, 0));
                })}
            };

            public override string NameID => "虚影";
        }
        private class 星命定轨 : AbstractCardSkill
        {
            public override int[] Costs => new int[] { 0, 0, 3 };
            public override string NameID => "星命定轨";
            public override string[] SpecialTags => new string[] { TCGBase.Tags.SkillTags.Q };

            public override void AfterUseAction(AbstractTeam me, int[]? targetArgs = null)
            {
                me.Enemy.Hurt(new DamageVariable(DamageSource.Character, 2, 4, 0));
                me.AddPersistent(new 泡影());
            }
            private class 泡影 : AbstractCardPersistentEffect
            {
                public override int MaxUseTimes => 1;

                public override Dictionary<string, PersistentTrigger> TriggerDic => new() {
                    { TCGBase.Tags.SenderTags.HURT_MUL,new 泡影_Trigger()} };

                public override string NameID => "泡影";
                private class 泡影_Trigger : PersistentTrigger
                {
                    public void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
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
