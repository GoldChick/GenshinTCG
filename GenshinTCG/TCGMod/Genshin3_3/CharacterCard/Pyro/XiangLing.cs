﻿using TCGBase;

namespace Genshin3_3
{
    public class XiangLing : AbstractCardCharacter
    {
        public override AbstractCardSkill[] Skills => new AbstractCardSkill[]
        {
        new CharacterSimpleA("白案功夫",0,2,3),
        new CharacterSingleSummonE("锅巴出击",new 锅巴(),3),
        new CharacterEffectQ("旋火轮",3,3,new 火轮(),false,3,4)
        };

        public override string NameID => "xiangling";

        public override ElementCategory CharacterElement => ElementCategory.Pyro;

        public override WeaponCategory WeaponCategory => WeaponCategory.LONGWEAPON;

        public override CharacterRegion CharacterRegion => CharacterRegion.LIYUE;
        private class 火轮 : AbstractCardPersistentEffect
        {
            public override int MaxUseTimes => 2;

            public override PersistentTriggerDictionary TriggerDic => new()
            {
                { SenderTag.AfterUseSkill.ToString(),(me, p, s, v) =>
                    {
                        if(s is UseSkillSender ski && ski.Skill.NameID != "旋火轮")
                        {
                            me.Enemy.Hurt(new DamageVariable(3, 2, 0), this); 
                            p.AvailableTimes --;
                        }
                    }
                }
            };


            public override string NameID => "火轮";
        }
    }
    public class 锅巴 : AbstractCardPersistentSummon
    {
        public override int MaxUseTimes => 2;

        public override PersistentTriggerDictionary TriggerDic => new() {
            { SenderTag.RoundOver.ToString(),(me, p, s, v) => { me.Enemy.Hurt(new(3, 2, 0), this); p.AvailableTimes --; }}};

        public override string NameID => "guoba";
    }
}