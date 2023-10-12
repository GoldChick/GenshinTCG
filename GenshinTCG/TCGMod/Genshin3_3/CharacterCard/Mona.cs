using TCGCard;
using TCGGame;
using TCGMod;
using TCGBase;

namespace Genshin3_3
{
    public class Mona : AbstractCardCharacter
    {
        public override string MainElement => TCGBase.Tags.ElementTags.HYDRO;

        public override int MaxMP => 3;

        public override AbstractCardSkill[] Skills => new AbstractCardSkill[] {
            new CharacterTrivalNormalAttack("因果点破",2,1),
            new CharacterSingleSummonE("水中幻愿",2,1,new 虚影()) };

        public override string NameID => "mona";

        public override string[] Tags => new string[] { 
            TCGBase.Tags.CardTags.RegionTags.MONDSTADT,
            TCGBase.Tags.CardTags.WeaponTags.CATALYST,
            TCGBase.Tags.CardTags.CharacterTags.HUMAN};
        private class 虚影 : AbstractCardPersistentSummon
        {
            public override int InitialUseTimes => 1;
            public override bool DeleteWhenUsedUp => false;
            public override int MaxUseTimes => 1;
            public override Dictionary<string, IPersistentTrigger> TriggerDic => new()
                { { TCGBase.Tags.SenderTags.HURT_ADD, new PersistentPurpleShield(1, 1) },
                  { TCGBase.Tags.SenderTags.ROUND_OVER, new 虚影_Trigger()} };

            public override string NameID => "虚影";
            public override string[] Tags => Array.Empty<string>();
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
