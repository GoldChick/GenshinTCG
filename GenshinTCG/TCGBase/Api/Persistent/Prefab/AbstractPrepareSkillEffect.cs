namespace TCGBase
{
    /// <summary>
    /// 准备技能通过effect的形式给出(必须附属在角色本身)
    /// </summary>
    public abstract class AbstractPrepareSkillEffect : AbstractCardPersistent
    {
        public override int MaxUseTimes => 1;
        /// <summary>
        /// 这个没用，不要在hurt()中使用this，而应该去找指定的技能
        /// </summary>
        public sealed override DamageSource DamageSource => DamageSource.Character;

        private readonly Action<PlayerTeam, AbstractPersistent, AbstractSender, AbstractVariable?> _action;
        /// <summary>
        /// 准备技能使用成功后默认消除
        /// </summary>
        public AbstractPrepareSkillEffect(Action<PlayerTeam, AbstractPersistent, AbstractSender, AbstractVariable?> action)
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
                    me.Game.HandleEvent(new NetEvent(new NetAction(ActionType.Break)),me.TeamIndex);
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
