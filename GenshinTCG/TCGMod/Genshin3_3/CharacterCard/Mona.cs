using TCGCard;
using TCGGame;
using TCGUtil;
using TCGMod;
using TCGBase;

namespace Genshin3_3
{
    public class Mona : ICardCharacter
    {
        public string MainElement => TCGBase.Tags.ElementTags.HYDRO;

        public int MaxHP => 10;

        public int MaxMP => 3;

        public IEffect? DefaultEffect => null;

        public ICardSkill[] Skills => new ICardSkill[] { new 因果点破(), new 水中幻愿() };

        public string NameID => "mona";

        public string[] Tags => new string[] { TCGBase.Tags.CardTags.RegionTags.MONDSTADT,
        TCGBase.Tags.CardTags.WeaponTags.CATALYST,TCGBase.Tags.CardTags.CharacterTags.HUMAN};
        private class 因果点破 : ICardSkill
        {
            public string NameID => "yinguodianpo";

            public string[] Tags => new string[] { TCGBase.Tags.SkillTags.NORMAL_ATTACK };

            public int[] Costs => new int[] { 1 };

            public bool CostSame => false;

            public void AfterUseAction(AbstractTeam me, int[]? targetArgs = null)
            {
                me.Enemy.Hurt(new(2, 1, DamageSource.Character, 0));
            }
        }
        private class 水中幻愿 : ICardSkill, ISummonProvider
        {
            public string NameID => "水中幻愿";

            public string[] Tags => new string[] { TCGBase.Tags.SkillTags.E };

            public int[] Costs => new int[] { 1 };

            public bool CostSame => false;

            public ISummon[] PersistentPool => new[] { new 虚影() };

            public bool PersistentOrdered => true;

            public int PersistentNum => 1;

            public void AfterUseAction(AbstractTeam me, int[]? targetArgs = null)
            {
                Logger.Warning("莫娜使用了水中幻愿!");
                me.Enemy.Hurt(new(2, 1, DamageSource.Character, 0));
                me.TryAddSummon(this);
            }
            private class 虚影 : ISummon
            {
                public int InitialUseTimes => 1;

                public bool DeleteWhenUsedUp => false;

                public int MaxUseTimes => 1;

                public Dictionary<string, IPersistentTrigger> TriggerDic => new()
                { { TCGBase.Tags.SenderTags.HURT_ADD, new PersistentPurpleShield(1, 1) },
                  { TCGBase.Tags.SenderTags.ROUND_OVER, new 虚影_Trigger()} };

                public string NameID => "虚影";

                public string[] Tags => Array.Empty<string>();
                private class 虚影_Trigger : IPersistentTrigger
                {
                    public void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
                    {
                        persitent.Active = false;
                        me.Enemy.Hurt(new(2, 1, DamageSource.Summon, 0));
                    }
                }
            }
        }
    }
}
