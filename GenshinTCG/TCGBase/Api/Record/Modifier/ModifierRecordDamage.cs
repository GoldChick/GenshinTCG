
namespace TCGBase
{
    public record class ModifierRecordDamage : ModifierRecordBase
    {
        public ModifierRecordDamage(ModifierType type, int value, ModifierMode mode = ModifierMode.Add, int consume = 1, List<ConditionRecordBase>? when = null, List<TargetRecord>? whenwith = null) : base(type, value, mode, consume, when, whenwith)
        {
        }

        protected override EventPersistentHandler? Get()
        {
            EventPersistentHandler handler = (me, p, s, v) =>
            {
                if (s is AbstractUseDiceSender uds)
                {

                }
            };
            return handler;
        }
    }
}
