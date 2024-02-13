
namespace TCGBase
{
    public record class ModifierRecordDamage : ModifierRecordBase
    {
        public ModifierRecordDamage(ModifierType type, int value, ModifierMode mode = ModifierMode.Add, int consume = 1, List<List<ConditionRecordBase>>? whenany = null) : base(type, value, mode, consume, whenany)
        {
        }

        protected override EventPersistentHandler? Get()
        {
            EventPersistentHandler handler = (me, p, s, v) =>
            {

            };
            return handler;
        }
    }
}
