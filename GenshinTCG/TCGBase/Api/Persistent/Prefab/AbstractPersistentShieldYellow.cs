namespace TCGBase
{
    /// <summary>
    /// 能被双岩增加的abstract护盾类<br/>
    /// 添加黄盾请考虑继承此类
    /// </summary>
    public abstract class AbstractPersistentShieldYellow : AbstractCardPersistentSummon
    {
        public override sealed PersistentTriggerDictionary TriggerDic { get; }
        public AbstractPersistentShieldYellow(Action<PlayerTeam, AbstractPersistent, AbstractSender, AbstractVariable?>? aftertriggeraction = null)
        {
            TriggerDic = new()
            {
                { SenderTag.HurtDecrease.ToString(),(me,p,s,v)=>
                {
                    if (p.AvailableTimes > 0 && v is DamageVariable dv && s.TeamID == me.TeamIndex)
                    {
                        if (p is not PersonalEffect || me.CurrCharacter == p.PersistentRegion)
                        {
                            if (dv.Element >= 0)
                            {
                                int a = int.Min(p.AvailableTimes, dv.Damage);
                                dv.Damage -= a;
                                p.AvailableTimes -= a;
                                aftertriggeraction?.Invoke(me,p,s,v);
                            }
                        }
                    }
                }
                }
            };
        }
    }
}
