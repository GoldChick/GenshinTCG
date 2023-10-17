using TCGBase;
using TCGCard;
using TCGGame;
using TCGMod;

namespace Genshin3_3
{
    public class Sucrose : AbstractCardCharacter
    {
        public override AbstractCardSkill[] Skills => new AbstractCardSkill[] {
        new CharacterTrivalNormalAttack("简式风灵作成",7,1),

        };

        public override string NameID => "sucrose";

        public override ElementCategory CharacterElement => throw new NotImplementedException();

        public override WeaponCategory WeaponCategory => throw new NotImplementedException();

        public override CharacterRegion CharacterRegion => throw new NotImplementedException();

        private class 风灵作成6308 : AbstractCardSkill
        {
            public override int[] Costs => new int[] { 0, 0, 0, 0, 0, 0, 0, 1 };

            public override bool CostSame => true;

            public override string NameID => "风灵作成·陆叁零捌";

            public override string[] SpecialTags => new string[] { TCGBase.Tags.SkillTags.E };

            public override void AfterUseAction(AbstractTeam me, int[]? targetArgs = null)
            {
                me.Enemy.Hurt(new TCGBase.DamageVariable(TCGBase.DamageSource.Character, 7, 3, 0), me.Enemy.SwitchToLast);
            }
        }
    }
}
