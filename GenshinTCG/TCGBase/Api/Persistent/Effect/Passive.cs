namespace TCGBase
{
    /// <summary>
    /// 用于触发被动技能,但里面没有检测天赋，需要自己额外写（？）
    /// </summary>
    internal class Passive : AbstractCardPersistent
    {
        private readonly PersistentTriggerDictionary _triggerDic;
        //TODO namespace?
        //public override string Namespace => "equipment";
        public override string NameID { get; }
        public Passive(AbstractPassiveSkill skill, int chaIndex, AbstractCardCharacter cha)
        {
            NameID = $"passive_{cha.Namespace}_{cha.NameID}";
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
