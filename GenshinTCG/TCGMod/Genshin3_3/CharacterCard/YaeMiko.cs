using TCGBase;
using TCGCard;
using TCGGame;
using TCGMod;

namespace Genshin3_3
{
    public class YaeMiko : AbstractCardCharacter
    {
        public override AbstractCardSkill[] Skills => new AbstractCardSkill[] {
        new CharacterSimpleA("胡灵验最是",4,1),
        new CharacterSingleSummonE("召唤杀生樱",new 杀生樱(),4),
        new 大秘法天胡县镇()
        };

        public override string NameID => "yaemiko";

        public override ElementCategory CharacterElement => ElementCategory.ELECTRO;

        public override CharacterCategory CharacterCategory => CharacterCategory.HUMAN;

        public override CharacterRegion CharacterRegion => CharacterRegion.INAZUMA;

        public override WeaponCategory WeaponCategory => WeaponCategory.CATALYST;

        private class 大秘法天胡县镇 : AbstractCardSkill
        {
            public override int[] Costs => new int[] { 1 };

            public override string NameID => "大秘法天湖县镇";

            public override SkillCategory Category => SkillCategory.Q;

            public override void AfterUseAction(PlayerTeam me, Character c, int[]? targetArgs = null)
            {
                me.Enemy.Hurt(new DamageVariable(4, 4, 0), this);
                var s = me.Summons.TryGet("杀生樱");
                if (s != null)
                {
                    s.AvailableTimes = 0;
                }
            }
        }

        private class 杀生樱 : AbstractCardPersistentSummon
        {
            public override int InitialUseTimes => 3;
            public override int MaxUseTimes => 6;
            public override void Update(AbstractPersistent<AbstractCardPersistentSummon> persistent)
            {
                if (persistent.AvailableTimes < MaxUseTimes)
                {
                    persistent.AvailableTimes = int.Min(persistent.AvailableTimes + 3, MaxUseTimes);
                }
            }

            public override PersistentTriggerDictionary TriggerDic => new()
            {
                {Tags.SenderTags.AFTER_PASS,(me,p,s,v)=>
                {
                    if (p.AvailableTimes>3 &&s.TeamID==me.TeamIndex)
                    {
                        p.AvailableTimes--;
                        me.Enemy.Hurt(new(4, 1, 0), this);
                    }}},
                 { Tags.SenderTags.ROUND_OVER,(me, p, s, v) => { p.AvailableTimes --; me.Enemy.Hurt(new(4, 1, 0), this); }}
            };

            public override string NameID => "杀生樱";

        }
    }
}
