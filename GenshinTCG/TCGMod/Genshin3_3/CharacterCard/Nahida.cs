using TCGBase;
using TCGCard;
using TCGGame;
using TCGMod;
using TCGUtil;

namespace Genshin3_3
{
    public class Nahida : AbstractCardCharacter
    {
        public override AbstractCardSkill[] Skills => new AbstractCardSkill[] {
        new CharacterSimpleA("行相",6,1),
        new 所闻编辑真如()
        };

        public override ElementCategory CharacterElement => ElementCategory.DENDRO;

        public override WeaponCategory WeaponCategory => WeaponCategory.CATALYST;

        public override CharacterRegion CharacterRegion => CharacterRegion.SUMERU;

        public override string NameID => "nahida";
        public class 所闻编辑 : AbstractCardSkill
        {
            public override int[] Costs => throw new NotImplementedException();

            public override string NameID => throw new NotImplementedException();

            public override SkillCategory Category => throw new NotImplementedException();

            public override void AfterUseAction(AbstractTeam me, Character c, int[]? targetArgs = null)
            {
                throw new NotImplementedException();
            }
        }
        public class 所闻编辑真如 : AbstractCardSkill
        {
            public override int[] Costs => new int[] { 1 };

            public override string NameID => "所闻编辑真如";

            public override SkillCategory Category => SkillCategory.E;

            public override void AfterUseAction(AbstractTeam me, Character c, int[]? targetArgs = null)
            {
                for (int i = 0; i < me.Enemy.Characters.Length; i++)
                {
                    me.Enemy.AddPersistent(new 怨种印(), i);
                }
                me.Enemy.Hurt(new DamageVariable(6, 3, 0), this);
            }
        }
        public class 怨种印 : AbstractCardPersistentEffect
        {
            private readonly static string 怨种印触发 = "genshin3_3:怨种印触发";
            public override int MaxUseTimes => 2;

            public override Dictionary<string, PersistentTrigger> TriggerDic => new()
            {
                {
                    Tags.SenderTags.AFTER_HURT,
                    new((me, p, s, v) =>
                    {
                        if (s is HurtSender hs &&  hs.Reaction!=null)
                        {
                            if (hs.TeamID==me.TeamIndex && hs.TargetIndex==p.PersistentRegion)
                            {
                                p.AvailableTimes--;
                                p.Data=1;
                                me.Hurt(new DamageVariable(-1,1,0),this);
                                me.EffectTrigger(me.Game,me.TeamIndex,new SimpleSender(怨种印触发));
                            }
                        }
                }) },
                {
                    怨种印触发,
                    new((me, p, s, v) =>
                    {
                        if (p.Data!=null && p.Data.Equals(1))
                        {
                            p.Data=null;
                        }else
                        {
                                p.AvailableTimes--;
                                p.Data=1;
                                me.Hurt(new DamageVariable(-1,1,p.PersistentRegion-me.CurrCharacter),this);
                        }
                }) }
            };

            public override string NameID => "怨种印";
        }
    }
}
