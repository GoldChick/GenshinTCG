namespace TCGBase
{
    public interface IPersistentTrigger
    {
        public SenderTag Tag { get; }
        /// <summary>
        /// 结算或预结算一次效果<br/>
        /// 次数的减少需要自己维护
        /// </summary>
        /// <param name="persitent">当前触发效果的persistent对应的object,用来减少、增加次数</param>
        /// <param name="sender">信息的发送者,如打出的[牌],使用的[技能]</param>
        /// <param name="variable">可以被改写的东西,如[消耗的骰子们],[伤害] <b>(不应改变类型)</b></param>
        public void Trigger(PlayerTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable);
    }
    public interface IMultiPersistentTrigger
    {
        public List<IPersistentTrigger> Triggers { get; }
    }
    public static class PersistentPreset
    {
        public class AfterUseSkill : IPersistentTrigger
        {
            public SenderTag Tag => SenderTag.AfterUseSkill;
            public Action<PlayerTeam, AbstractPersistent, AfterUseSkillSender, AbstractVariable?> _action;
            public AfterUseSkill(Action<PlayerTeam, AbstractPersistent, AfterUseSkillSender, AbstractVariable?> action)
            {
                _action = action;
            }
            public void Trigger(PlayerTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
            {
                if (persitent.Data != null && sender is AfterUseSkillSender ski)
                {
                    _action.Invoke(me, persitent, ski, variable);
                }
                persitent.Data = 1;
            }
        }
        public class AfterSkillTriggered : IPersistentTrigger
        {
            public SenderTag Tag => SenderTag.AfterUseSkill;
            public Action<PlayerTeam, AbstractPersistent, AfterUseSkillSender, AbstractVariable?> _action;
            public AfterSkillTriggered(Action<PlayerTeam, AbstractPersistent, AfterUseSkillSender, AbstractVariable?> action)
            {
                _action = action;
            }
            public void Trigger(PlayerTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
            {
                if (sender is AfterUseSkillSender ski)
                {
                    _action.Invoke(me, persitent, ski, variable);
                }
            }
        }
        /// <summary>
        /// 回合交替时刷新次数为最大可用
        /// </summary>
        public class RoundStepReset : IPersistentTrigger
        {
            public SenderTag Tag => SenderTag.RoundStep;

            public void Trigger(PlayerTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
            {
                persitent.AvailableTimes = persitent.CardBase.MaxUseTimes;
            }
        }
        public class RoundStepDecrease : IPersistentTrigger
        {
            public SenderTag Tag => SenderTag.RoundStep;

            public void Trigger(PlayerTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
            {
                persitent.AvailableTimes--;
            }
        }
        public class WeaponDamageIncrease : IPersistentTrigger
        {
            public SenderTag Tag => SenderTag.DamageIncrease;
            private readonly int _increase;
            private readonly Func<PlayerTeam, AbstractPersistent, PreHurtSender, DamageVariable, int>? _dynamic_increase;
            public WeaponDamageIncrease(int increase = 1)
            {
                _increase = increase;
            }
            public WeaponDamageIncrease(Func<PlayerTeam, AbstractPersistent, PreHurtSender, DamageVariable, int> dynamic_increase)
            {
                _dynamic_increase = dynamic_increase;
            }
            public void Trigger(PlayerTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
            {
                if (sender.TeamID == me.TeamIndex && variable is DamageVariable dv && dv.DirectSource == DamageSource.Character && sender is PreHurtSender hs)
                {
                    if (persitent.PersistentRegion < 0 || persitent.PersistentRegion > 10 || me.CurrCharacter == persitent.PersistentRegion)
                    {
                        dv.Damage += _dynamic_increase?.Invoke(me, persitent, hs, dv) ?? _increase;
                    }
                }
            }
        }
        /// <summary>
        /// 默认_condition为me.TeamIndex == sender.TeamID<br/>
        /// 默认_costmodifier为1个有效骰<br/>
        /// 默认_aftertriggeraction为-1可用次数
        /// </summary>
        public class UseDiceModifier<T> : IPersistentTrigger where T : AbstractUseDiceSender
        {
            public SenderTag Tag { get; }
            private readonly Func<PlayerTeam, AbstractPersistent, T, CostVariable, bool> _condition;
            private readonly Func<PlayerTeam, AbstractPersistent, T, CostVariable, CostModifier> _costmodifier;
            private readonly Action<PlayerTeam, AbstractPersistent, T, CostVariable> _aftertriggeraction;
            /// <summary>
            /// 默认_condition为me.TeamIndex == sender.TeamID<br/>
            /// 默认_costmodifier为1个有效骰<br/>
            /// 默认_aftertriggeraction为-1可用次数<br/>
            /// <b>如果想使用默认值，就置为null</b>
            /// </summary>
            public UseDiceModifier(Func<PlayerTeam, AbstractPersistent, T, CostVariable, bool>? condition = null,
                Func<PlayerTeam, AbstractPersistent, T, CostVariable, CostModifier>? costmodifier = null,
                Action<PlayerTeam, AbstractPersistent, T, CostVariable>? aftertriggeraction = null)
            {
                Tag = typeof(T).Name.ToString() switch
                {
                    "UseDiceFromCardSender" => SenderTag.UseDiceFromCard,
                    "UseDiceFromSkillSender" => SenderTag.UseDiceFromSkill,
                    _ => SenderTag.UseDiceFromSwitch
                };
                _condition = condition ?? ((me, p, s, v) => me.TeamIndex == s.TeamID);
                _costmodifier = costmodifier ?? ((me, p, s, v) => new CostModifier(DiceModifierType.Same, 1));
                _aftertriggeraction = aftertriggeraction ?? ((me, p, s, v) => p.AvailableTimes--);
            }
            public void Trigger(PlayerTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
            {
                if (sender is T ss && variable is CostVariable cv)
                {
                    if (_condition.Invoke(me, persitent, ss, cv) && _costmodifier.Invoke(me, persitent, ss, cv).Modifier(cv))
                    {
                        _aftertriggeraction.Invoke(me, persitent, ss, cv);
                    }
                }
            }
        }
    }
}
