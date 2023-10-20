using TCGBase;
using TCGCard;
using TCGMod;

namespace Genshin3_3
{
    public class Qiqi : AbstractCardCharacter
    {
        public override AbstractCardSkill[] Skills => new AbstractCardSkill[] {
        new CharacterSimpleA("古运来剑法",1,2),
        new CharacterSingleSummonE("寒冰鬼差",new 寒冰鬼差(),1),
        };

        public override ElementCategory CharacterElement => ElementCategory.CRYO;

        public override WeaponCategory WeaponCategory => WeaponCategory.SWORD;

        public override CharacterRegion CharacterRegion => CharacterRegion.LIYUE;

        public override string NameID => "qiqi";
        public class 寒冰鬼差 : AbstractCardPersistentSummon
        {
            public override int MaxUseTimes => 3;

            public override Dictionary<string, PersistentTrigger> TriggerDic => new()
            {
                { Tags.SenderTags.ROUND_OVER,new((me,p,s,v)=>
                    {
                        //TODO:治疗效果
                        p.AvailableTimes--;
                        me.Enemy.Hurt(new DamageVariable(1,1,0),this);
                    })}
            };

            public override string NameID => "寒冰鬼差";
        }

    }
}
