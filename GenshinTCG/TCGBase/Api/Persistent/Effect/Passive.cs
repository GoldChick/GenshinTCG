﻿namespace TCGBase
{
    /// <summary>
    /// 用于触发被动技能,但里面没有检测天赋，需要自己额外写（？）
    /// TODO:天赋
    /// </summary>
    internal class Passive : AbstractCardPersistentEffect
    {
        private readonly PersistentTriggerDictionary _triggerDic;
        public Passive(AbstractPassiveSkill skill, int chaIndex)
        {
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
    }
}
