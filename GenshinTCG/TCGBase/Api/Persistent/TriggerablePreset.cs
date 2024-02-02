namespace TCGBase
{
    public interface ITriggerable
    {
        public string Tag { get; }
        /// <summary>
        /// 结算或预结算一次效果<br/>
        /// 次数的减少需要自己维护
        /// </summary>
        /// <param name="persitent">当前触发效果的persistent对应的object,用来减少、增加次数</param>
        /// <param name="sender">信息的发送者,如打出的[牌],使用的[技能]</param>
        /// <param name="variable">可以被改写的东西,如[消耗的骰子们],[伤害] <b>(不应改变类型)</b></param>
        public void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable);
    }
    internal class Triggerable : ITriggerable
    {
        public EventPersistentHandler Handler;
        public string Tag { get; }
        public Triggerable(string tag, EventPersistentHandler h)
        {
            Tag = tag;
            Handler = h;
        }
        public void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable) => Handler.Invoke(me, persitent, sender, variable);
    }

    //public static class TriggerablePreset
    //{
    //    public class AfterUseSkill : ITriggerable
    //    {
    //        public string Tag => SenderTag.AfterUseSkill.ToString();
    //        public Action<AbstractTeam, AbstractPersistent, AfterUseSkillSender, AbstractVariable?> _action;
    //        public AfterUseSkill(Action<AbstractTeam, AbstractPersistent, AfterUseSkillSender, AbstractVariable?> action)
    //        {
    //            _action = action;
    //        }
    //        public void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
    //        {
    //            if (persitent.Data != null && sender is AfterUseSkillSender ski)
    //            {
    //                _action.Invoke(me, persitent, ski, variable);
    //            }
    //            persitent.Data = 1;
    //        }
    //    }
    //    public class AfterSkillTriggered : ITriggerable
    //    {
    //        public string Tag => SenderTag.AfterUseSkill.ToString();
    //        public Action<AbstractTeam, AbstractPersistent, AfterUseSkillSender, AbstractVariable?> _action;
    //        public AfterSkillTriggered(Action<AbstractTeam, AbstractPersistent, AfterUseSkillSender, AbstractVariable?> action)
    //        {
    //            _action = action;
    //        }
    //        public void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
    //        {
    //            if (sender is AfterUseSkillSender ski)
    //            {
    //                _action.Invoke(me, persitent, ski, variable);
    //            }
    //        }
    //    }
    //    /// <summary>
    //    /// 回合交替时刷新次数为最大可用
    //    /// </summary>
    //    public class RoundStepReset : ITriggerable
    //    {
    //        public string Tag => SenderTag.RoundStep.ToString();

    //        public void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
    //        {
    //            persitent.AvailableTimes = persitent.CardBase.MaxUseTimes;
    //        }
    //    }
    //    public class RoundStepDecrease : ITriggerable
    //    {
    //        public string Tag => SenderTag.RoundStep.ToString();

    //        public void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
    //        {
    //            persitent.AvailableTimes--;
    //        }
    //    }
    //    public class WeaponDamageIncrease : ITriggerable
    //    {
    //        public string Tag => SenderTag.DamageIncrease.ToString();
    //        private readonly int _increase;
    //        private readonly Func<AbstractTeam, AbstractPersistent, PreHurtSender, DamageVariable, int>? _dynamic_increase;
    //        public WeaponDamageIncrease(int increase = 1)
    //        {
    //            _increase = increase;
    //        }
    //        public WeaponDamageIncrease(Func<AbstractTeam, AbstractPersistent, PreHurtSender, DamageVariable, int> dynamic_increase)
    //        {
    //            _dynamic_increase = dynamic_increase;
    //        }
    //        public void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
    //        {
    //            if (sender.TeamID == me.TeamIndex && variable is DamageVariable dv && dv.DirectSource == DamageSource.Character && sender is PreHurtSender hs)
    //            {
    //                if (persitent.PersistentRegion < 0 || persitent.PersistentRegion > 10 || me.CurrCharacter == persitent.PersistentRegion)
    //                {
    //                    dv.Damage += _dynamic_increase?.Invoke(me, persitent, hs, dv) ?? _increase;
    //                }
    //            }
    //        }
    //    }
    //    public class UseDiceModifier<T> : ITriggerable where T : AbstractUseDiceSender
    //    {
    //        public string Tag { get; }
    //        private readonly Func<AbstractTeam, AbstractPersistent, T, CostVariable, bool> _condition;
    //        private readonly Func<AbstractTeam, AbstractPersistent, T, CostVariable, CostModifier> _costmodifier;
    //        private readonly Action<AbstractTeam, AbstractPersistent, T, CostVariable>? _aftertriggeraction;
    //        /// <summary>
    //        /// 默认_condition为me.TeamIndex == sender.TeamID<br/>
    //        /// 默认_costmodifier为1个有效骰<br/>
    //        /// 默认_aftertriggeraction为空<br/>
    //        /// <b>默认decreaseAvailabletimes为true，即成功触发后会减少次数（在aftertriggeraction之后）</b>
    //        /// <b>如果想使用默认值，就置为null</b>
    //        /// </summary>
    //        public UseDiceModifier(Func<AbstractTeam, AbstractPersistent, T, CostVariable, bool>? condition = null,
    //            Func<AbstractTeam, AbstractPersistent, T, CostVariable, CostModifier>? costmodifier = null,
    //            Action<AbstractTeam, AbstractPersistent, T, CostVariable>? aftertriggeraction = null,
    //            bool decreaseAvailabletimes = true)
    //        {
    //            Tag = typeof(T).Name.ToString() switch
    //            {
    //                "UseDiceFromCardSender" => SenderTag.UseDiceFromCard.ToString(),
    //                "UseDiceFromSkillSender" => SenderTag.UseDiceFromSkill.ToString(),
    //                _ => SenderTag.UseDiceFromSwitch.ToString()
    //            };
    //            _condition = condition ?? ((me, p, s, v) => me.TeamIndex == s.TeamID);
    //            _costmodifier = costmodifier ?? ((me, p, s, v) => new CostModifier(ElementCategory.Trival, 1));
    //            _aftertriggeraction = aftertriggeraction;
    //            if (decreaseAvailabletimes)
    //            {
    //                _aftertriggeraction += ((me, p, s, v) => p.AvailableTimes--);
    //            }
    //        }
    //        public void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
    //        {
    //            if (sender is T ss && variable is CostVariable cv)
    //            {
    //                if (_condition.Invoke(me, persitent, ss, cv) && _costmodifier.Invoke(me, persitent, ss, cv).Modifier(cv))
    //                {
    //                    _aftertriggeraction?.Invoke(me, persitent, ss, cv);
    //                }
    //            }
    //        }
    //    }
    //    public class HurtDecreasePurpleShield : ITriggerable
    //    {
    //        private readonly int _line;
    //        private readonly int _protect;
    //        /// <summary>
    //        /// 我方[附属该[角色状态]的角色]/[角色]受到伤害时，如果此状态可用次数>0，并且受到不为穿透伤害就触发；一次只能消耗一个次数<br/><br/>
    //        /// line 结算到此buff，伤害大于等于_line时才触发<br/>
    //        /// protectNum 一次抵挡多少伤害
    //        ///  </summary>
    //        public HurtDecreasePurpleShield(int protectNum, int line = 1)
    //        {
    //            _line = line;
    //            _protect = protectNum;
    //        }

    //        public string Tag => SenderTag.HurtDecrease.ToString();

    //        public void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
    //        {
    //            if (persitent.AvailableTimes > 0 && sender.TeamID == me.TeamIndex && variable is DamageVariable dv)
    //            {
    //                if (persitent.PersistentRegion < 0 || persitent.PersistentRegion > 10 || me.CurrCharacter == persitent.PersistentRegion)
    //                {
    //                    if (dv.Element >= 0 && dv.Damage >= _line)
    //                    {
    //                        dv.Damage -= _protect;
    //                        persitent.AvailableTimes--;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    public class HurtDecreaseYellowShield : ITriggerable
    //    {
    //        public HurtDecreaseYellowShield()
    //        {
    //        }

    //        public string Tag => SenderTag.HurtDecrease.ToString();

    //        public void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
    //        {
    //            if (persitent.AvailableTimes > 0 && sender.TeamID == me.TeamIndex && variable is DamageVariable dv)
    //            {
    //                if (persitent.PersistentRegion < 0 || persitent.PersistentRegion > 10 || me.CurrCharacter == persitent.PersistentRegion)
    //                {
    //                    if (dv.Element >= 0)
    //                    {
    //                        int a = int.Min(persitent.AvailableTimes, dv.Damage);
    //                        dv.Damage -= a;
    //                        persitent.AvailableTimes -= a;
    //                        //aftertriggeraction?.Invoke(me, p, s, v);
    //                    }
    //                }
    //            }
    //        }
    //    }

    //}

}
