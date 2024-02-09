
namespace TCGBase
{
    public record class ActionRecordSwitch : ActionRecordBase
    {
        //TODO

        /// <summary>
        /// 相对坐标
        /// </summary>
        public int To { get; }
        public ActionRecordSwitch() : base(TriggerType.Switch)
        {
        }
        public override EventPersistentHandler? GetHandler(ITriggerable triggerable)
        {
            return (me, p, s, v) =>
            {
            };
        }
    }
}
