using System.Collections.Immutable;
using TCGBase;
using TCGGame;

namespace TCGMod
{
    /// <summary>
    /// 用于触发被动技能
    /// </summary>
    internal class Passive : AbstractCardPersistentEffect
    {
        private readonly string _nameID;
        private readonly PersistentTriggerDictionary _triggerDic;
        public Passive(AbstractPassiveSkill skill, int chaIndex)
        {
            _nameID = skill.NameID;
            _triggerDic = new(skill.TriggerDic.ToDictionary<string, string, EventPersistentHandler>(st => st, st =>
                (PlayerTeam me, AbstractPersistent p, AbstractSender s, AbstractVariable? v) =>
                {
                     skill.AfterUseAction(me, me.Characters[chaIndex], new int[] { s.TeamID });
                     if (skill.TriggerOnce)
                     {
                         p.AvailableTimes--;
                     }
                })
            );
        }
        public override int MaxUseTimes => 1;
        public override PersistentTriggerDictionary TriggerDic => _triggerDic;
        public override string NameID => _nameID;
    }
}
