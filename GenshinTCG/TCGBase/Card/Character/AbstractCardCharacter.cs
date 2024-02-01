﻿namespace TCGBase
{
    public enum CharacterCategory
    {
        Human,
        Mob
    }
    public enum CharacterRegion
    {
        None,
        ABYSS,
        MONDSTADT,
        LIYUE,
        INAZUMA,
        SUMERU,
        FONTAINE,
        NATLAN,
        Fatui,
        /// <summary>
        /// 丘丘人也算个国家算了
        /// </summary>
        QQ
    }
    public abstract class AbstractCardCharacter : AbstractCardPersistent
    {
        public virtual int MaxHP { get => 10; }
        public virtual int MaxMP { get => 2; }
        /// <summary>
        ///  @NonNull 角色的所有技能
        /// </summary>
        public abstract AbstractCardSkill[] Skills { get; }
        /// <summary>
        /// 主元素，用于调和和携带共鸣牌的判定等
        /// </summary>
        public abstract ElementCategory CharacterElement { get; }
        /// <summary>
        /// 角色卡的武器类型
        /// </summary>
        public abstract WeaponCategory WeaponCategory { get; }
        /// <summary>
        /// 角色卡的户口
        /// </summary>
        public abstract CharacterRegion CharacterRegion { get; }
        public override sealed bool CustomDesperated => true;
        public override sealed int MaxUseTimes => 0;
        public override PersistentTriggerList TriggerList { get; }
        /// <summary>
        /// 角色卡的(生物)种类，默认为HUMAN人类
        /// </summary>
        public virtual CharacterCategory CharacterCategory => CharacterCategory.Human;
        protected AbstractCardCharacter()
        {
            TriggerList = new()
            {
                { SenderTag.RoundStart,(me,p,s,v)=>
                {
                    if (p is Character c)
                    {
                        c.SkillCounter.Clear();
                    }
                }
                },
                { SenderTagInner.Use,(me,p,s,v)=>
                {
                    if (p is Character c && s is ActionUseSkillSender ss &&  c.Card.GetType()==ss.Character.GetType())
                    {
                        if (ss.Skill>=0 && ss.Skill<Skills.Length && me is PlayerTeam pt)
                        {
                            var skill=Skills[ss.Skill];
                            if (skill.DamageSkillCategory!=SkillCategory.Q)
                            {
                                //TODO: talent override?
                                skill.AfterUseAction(pt,c);

                                c.SkillCounter[ss.Skill]++;
                                if (skill.Hidden)
                                {
                                    //TODO:天赋的ue skill
                                    me.Game.EffectTrigger(new AfterUseSkillSender(me.TeamIndex,c,skill));
                                }
                            }
                        }
                    }
                }
                }
            };
            //初始化被动技能 
            foreach (var s in Skills)
            {
                if (s is AbstractCardSkillPassive passive)
                {
                    if (TriggerList.TryGetValue(passive.TriggerSenderTag, out var ipt) && ipt is PersistentTriggerList.PersistentTrigger pt)
                    {
                        pt.Handler += passive.AfterTriggerAction;
                    }
                    else
                    {
                        TriggerList.Add(passive.TriggerSenderTag, passive.AfterTriggerAction);
                    }
                }
            }
            Tags.Add(CharacterElement.ToTags());
            Tags.Add(WeaponCategory.ToTags());
            Tags.Add(CharacterRegion.ToTags());
            Tags.Add(CharacterCategory.ToTags());
        }
    }
}
