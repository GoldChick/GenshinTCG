namespace TCGBase
{
    /// <summary>
    /// 准备技能通过effect的形式给出(必须附属在角色本身)
    /// </summary>
    public abstract class AbstractPrepareSkillEffect : AbstractCardPersistent
    {
        public override int MaxUseTimes => 1;

        private readonly Action<AbstractTeam, AbstractPersistent, AbstractSender, AbstractVariable?> _action;
        /// <summary>
        /// 准备技能使用成功后默认消除
        /// </summary>
        public AbstractPrepareSkillEffect(Action<AbstractTeam, AbstractPersistent, AbstractSender, AbstractVariable?> action)
        {
            _action = action;
        }
        public sealed override PersistentTriggerDictionary TriggerDic => new()
        {
            { SenderTag.RoundMeStart,(me,p,s,v)=>
            {
                if (me.TeamIndex==s.TeamID && me.Characters[p.PersistentRegion].Active)
                {
                    _action.Invoke(me,p,s,v);
                    p.Active=false;
                    //TODO: prepare skill?

                    //me.RealGame.HandleEvent(new NetEvent(new NetAction(ActionType.Break)),me.TeamIndex);
                }
            }
            },
            { SenderTag.AfterSwitch,(me,p,s,v)=>
            {
                if (me.TeamIndex==s.TeamID && s is AfterSwitchSender ss && ss.Target!=p.PersistentRegion)
                {
                    p.Active=false;
                }
            }
            }
        };
    }
}
