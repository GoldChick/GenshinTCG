using System.Text.Json.Serialization;

namespace TCGBase
{
    public enum TriggerableType
    {
        /// <summary>
        /// tag: <see cref="SenderTagInner.UseSkill"/>; 参数: [<see cref="SkillCategory"/>] [Cost] [Trigger]<br/>
        /// eg:  "skill[a[pyro=1,void=2[dodamageaorb,pyro-3,count=3,pyro-5[mp=1" 迪卢克e
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
    public record class TriggerableRecordBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TriggerableType Type { get; }

        public TriggerableRecordBase(TriggerableType type)
        {
            Type = type;
        }
    }
}
