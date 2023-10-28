using TCGBase;

namespace Genshin3_3
{
    /// <summary>
    /// 奥兹换班霜华矢，唯此一心冰灵珠！
    /// </summary>
    public class Ganyu : AbstractCardCharacter
    {
        public override int MaxMP => 1;
        public override AbstractCardSkill[] Skills => new AbstractCardSkill[] {
            new CharacterSimpleA("a",0,2,1),
            new CharacterEffectE("e",1,1,new 冰莲(),false),
            new CharacterSingleSummonQ("q",1,2,new SimpleSummon("冰灵珠",1,2,2)),
        };
        private class 冰莲 : AbstractCardPersistentEffect
        {
            public override int MaxUseTimes => 2;

            public override PersistentTriggerDictionary TriggerDic => new()
            {
                {SenderTag.HurtDecrease, new PersistentPurpleShield(1)}
            };

            public override string NameID => "冰莲";
        }
        private class 冰灵珠 : AbstractCardPersistentSummon
        {
            public override int MaxUseTimes => 2;

            public override PersistentTriggerDictionary TriggerDic => new()
            {
                { SenderTag.RoundOver,(me,p,s,v)=>
                    {
                        me.Enemy.MultiHurt(new DamageVariable[]{new(1,1,0),new(-1,1,0,true) },this);
                        p.AvailableTimes--;
                    }
                }
            };

            public override string NameID => "冰灵珠";
        }
        private class 霜华矢 : AbstractCardSkill
        {
            public override SkillCategory Category => SkillCategory.A;

            public override int[] Costs => new int[] { 0, 5 };

            public override string NameID => "霜华矢";

            public override void AfterUseAction(PlayerTeam me, Character c, int[]? targetArgs = null)
            {
                me.Enemy.MultiHurt(new DamageVariable[] { new(1, 2, 0), new(-1, 2, 0, true) }, this);
                me.AddPersistent(new SimpleEffect("涉过了"), c.Index);
            }
        }

        public override ElementCategory CharacterElement => ElementCategory.Cryo;

        public override WeaponCategory WeaponCategory => WeaponCategory.BOW;

        public override CharacterRegion CharacterRegion => CharacterRegion.LIYUE;

        public override string NameID => "ganyu";
    }
}
