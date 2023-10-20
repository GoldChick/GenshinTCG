using System.Collections.Immutable;
using TCGCard;

namespace TCGMod
{
    /// <summary>
    /// 用于触发被动技能
    /// </summary>
    internal class Passive : AbstractCardPersistentEffect
    {
        private readonly string _nameID;
        private readonly Dictionary<string, PersistentTrigger> _triggerDic;
        public Passive(AbstractPassiveSkill skill, int chaIndex)
        {
            _nameID = skill.NameID;
            _triggerDic = skill.TriggerDic.ToDictionary(s => s, s => new PersistentTrigger((me, p, s, v) =>
            {
                skill.AfterUseAction(me, me.Characters[chaIndex], new int[] { s.TeamID });
                if (skill.TriggerOnce)
                {
                    p.AvailableTimes--;
                }
            }
            ));
        }
        public override int MaxUseTimes => 1;
        public override Dictionary<string, PersistentTrigger> TriggerDic => _triggerDic;
        public override string NameID => _nameID;
    }
}
