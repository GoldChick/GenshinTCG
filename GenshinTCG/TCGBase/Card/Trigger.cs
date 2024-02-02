﻿namespace TCGBase
{
    /// <summary>
    /// 不同的参数之间用，分隔
    /// </summary>
    public enum TriggerType
    {
        /// <summary>
        /// 参数: dmg<br/>
        /// eg: "dodamage,pyro-3(-0-targetonly-enemy)" 班尼特e,括号内为可选项<br/>
        /// "dodamage,pierce-2-0-targetexcept+cryo-2" 甘雨5a
        /// </summary>
        DoDamage,
        /// <summary>
        /// 参数: dmgA condition dmgB<br/>
        /// eg:  "dodamageaorb,pyro-3,count=3,pyro-5" 迪卢克e
        /// </summary>
        DoDamageAorB,
        DoDamageAddEffect,
        DoDamageAddSummon,
        DoDamageWithSubDamage,
        DoDamageWithEffect,
        DoDamageWithSummon,
    }
    /// <summary>
    /// 不同的参数之间用-分隔
    /// </summary>
    public enum CharacterUseSkillType
    {
        /// <summary>
        /// 附属该状态的角色，若无，则为出战角色
        /// </summary>
        CurrCharacter,
        NotCurrCharacter,
        /// <summary>
        /// 我方所有角色
        /// </summary>
        AllMyCharacter,
        /// <summary>
        /// 对方所有角色
        /// </summary>
        AllEnemyCharacter,
        /// <summary>
        /// 场上所有角色
        /// </summary>
        AllGameCharacter,
        /// <summary>
        /// 指定我方角色; 参数:Character.NameID
        /// </summary>
        CertainCharacter,
        NotCertainCharacter,
    }
    public enum TriggerableType
    {
        /// <summary>
        /// tag: <see cref="SenderTagInner.Use"/>; 参数: [<see cref="SkillCategory"/>] [Cost] [Trigger]<br/>
        /// eg:  "skill[a[pyro=1,void=2[dodamageaorb,pyro-3,count=3,pyro-5" 迪卢克e
        /// </summary>
        Skill,
        /// <summary>
        /// tag: <see cref="SenderTag.AfterUseSkill"/>; 参数: [<see cref="SkillCategory"/>] [isonlyCurrCharacter] [Trigger]
        /// </summary>
        AfterUseSkill,
        /// <summary>
        /// tag: <see cref="SenderTag.AfterUseCard"/>; 参数:
        /// </summary>
        AfterUseCard,
        /// <summary>
        /// tag: <see cref="SenderTag.AfterBlend"/>;
        /// </summary>
        AfterBlend,
        /// <summary>
        /// tag: <see cref="SenderTag.AfterSwitch"/>;
        /// </summary>
        AfterSwitch,
        /// <summary>
        /// tag: <see cref="SenderTag.AfterHurt"/>;
        /// </summary>
        AfterHurt,
        /// <summary>
        /// tag: <see cref="SenderTag.AfterBlend"/>;
        /// </summary>
        AfterHealed,
    }
    public delegate void TriggerRegistry(params string[] args);
    public static class Trigger
    {
        public static ITriggerable Convert(string code)
        {
            try
            {
                var cs = code.Split('[');
                if (Enum.TryParse(cs[0], out TriggerableType type))
                {
                    return type switch
                    {
                        TriggerableType.Skill => TriggerPreset.DoSkill(cs),
                        _ => throw new Exception($"Trigger: 未检查的trigger:{cs[0]}")
                    };
                }
                else
                {
                    //TODO:custom trigger?
                    Console.WriteLine($"未注册的trigger:{cs[0]}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error In Trigger Convertion: {ex.Message}");
            }
            return null;
        }
    }
}
