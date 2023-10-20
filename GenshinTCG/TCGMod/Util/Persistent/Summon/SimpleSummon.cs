using TCGBase;
using TCGCard;

namespace TCGMod
{
    /// <summary>
    /// 结束时，对对方出战角色造成a点b元素伤害，初始可用次数=最大可用次数=c
    /// </summary>
    public class SimpleSummon : AbstractCardPersistentSummon
    {
        private readonly string _nameID;
        private readonly int _damage;
        private readonly int _element;
        private readonly int _maxusetimes;

        public SimpleSummon(string nameID,int element, int damage, int maxusetimes)
        {
            _nameID = nameID;
            _damage = damage;
            _element = element;
            _maxusetimes = maxusetimes;
        }
        public override int MaxUseTimes => _maxusetimes;

        public override Dictionary<string, PersistentTrigger> TriggerDic => new()
        {
            { Tags.SenderTags.ROUND_OVER,new((me,p,s,v)=>
                {
                    me.Enemy.Hurt(new(_element,_damage,0),this);
                    p.AvailableTimes--;
                }
            )}
        };

        public override string NameID => _nameID;
    }
}
