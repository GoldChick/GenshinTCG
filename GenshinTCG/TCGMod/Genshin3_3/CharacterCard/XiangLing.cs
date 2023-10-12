using TCGBase;
using TCGCard;
using TCGGame;
using TCGMod;
using TCGUtil;

namespace Genshin3_3
{
    public class XiangLing : AbstractCardCharacter
    {
        public override string MainElement => TCGBase.Tags.ElementTags.PYRO;

        public override AbstractCardSkill[] Skills => new AbstractCardSkill[] {
        new CharacterTrivalNormalAttack("白案功夫",0,2,3),
        new CharacterSingleSummonE("锅巴出击",new 锅巴(),3),
        new 旋火轮()};

        public override string NameID => "xiangling";

        public override string[] Tags => new string[] { "not curr" };

        private class 旋火轮 : AbstractCardSkill
        {
            //TODO: free temp
            public override int[] Costs => new int[] { };

            public override bool CostSame => true;

            public override string NameID => "旋火轮";

            public override string[] Tags => new string[] { TCGBase.Tags.SkillTags.NORMAL_ATTACK };

            public override void AfterUseAction(AbstractTeam me, int[]? targetArgs = null)
            {
                me.Enemy.Hurt(new DamageVariable(3, 3, DamageSource.Character, 0));
                me.AddPersistent(new 火轮());
            }
            private class 火轮 : AbstractCardPersistentEffect
            {
                public override int MaxUseTimes => 2;

                public override Dictionary<string, IPersistentTrigger> TriggerDic => new() {
                    { TCGBase.Tags.SenderTags.AFTER_USE_SKILL,new 火轮_Trigger()} };

                public override string NameID => "火轮";
                private class 火轮_Trigger : IPersistentTrigger
                {
                    public void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
                    {
                        if (sender is UseSkillSender ski)
                        {
                            if (ski.Skill is not 旋火轮)
                            {
                                Logger.Error("旋火轮触发了一次！");
                                persitent.AvailableTimes--;
                                me.Enemy.Hurt(new DamageVariable(3, 2, DamageSource.Addition, 0));
                            }
                        }
                    }
                }
            }
        }
    }
    public class 锅巴 : AbstractCardPersistentSummon
    {
        public override int MaxUseTimes => 2;

        public override Dictionary<string, IPersistentTrigger> TriggerDic => new() {
            { TCGBase.Tags.SenderTags.ROUND_OVER,new 虚影_Trigger()} };

        public override string NameID => "guoba";

        public override string[] Tags => new string[] { "i dont know" };
        private class 虚影_Trigger : IPersistentTrigger
        {
            public void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
            {
                persitent.AvailableTimes--;
                me.Enemy.Hurt(new(3, 2, DamageSource.Summon, 0));
            }
        }
    }
}
