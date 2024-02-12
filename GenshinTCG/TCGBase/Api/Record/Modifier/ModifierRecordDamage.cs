using System.Text.Json.Serialization;

namespace TCGBase
{
    //source: card/skill
    public record class ModifierRecordDamage : ModifierRecordBase
    {
        protected ModifierRecordDamage(ModifierRecordBase original) : base(original)
        {
        }
        public override EventPersistentHandler? Get()
        {
            return (me, p, s, v) =>
            {
                if (v is DamageVariable dv && s is PreHurtSender phs)
                {
                }
            };
        }
    }
    public enum DamageConditionType
    {

    }
    public record DamageCondition
    {
        public bool Direct;
        public int Element;
        public int Damage;
        public ReactionTags Reaction;
        public AbstractCustomTriggerable Source;
    }
}
