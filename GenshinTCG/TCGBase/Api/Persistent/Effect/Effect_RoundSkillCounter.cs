namespace TCGBase
{
    /// <summary>
    /// 每回合的技能计数器，角色使用技能后，附属一个这样的counter<br/>
    /// data: null or Dictionary<int,int><br/>
    /// 
    /// int1: skill index ; int2: used times (this round)
    /// </summary>
    public class Effect_RoundSkillCounter : AbstractCardPersistent
    {
        public int Skill { get; }
        public Effect_RoundSkillCounter(int skill)
        {
            Skill = skill;
            Variant = -4;
        }
        public override int MaxUseTimes => 1;
        public override PersistentTriggerDictionary TriggerDic => new()
        {
            { SenderTag.RoundStep,(me,p,s,v)=>p.Active=false},
            { SenderTag.RoundMeStart,(me,p,s,v)=>
            {
                if (p.Data==null && p.CardBase is Effect_RoundSkillCounter rsc)
                {
                    p.Data = new Dictionary<int, int>()
                    {
                        { rsc.Skill,1}
                    };
                }
            }
            }
        };
        public override void Update<T>(PlayerTeam me, Persistent<T> persistent)
        {
            if (persistent.Data is Dictionary<int, int> map)
            {
                if (!map.TryAdd(Skill, 1))
                {
                    map[Skill]++;
                }
            }
        }
    }
}
