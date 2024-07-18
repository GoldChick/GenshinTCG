namespace TCGBase
{
    /// <summary>
    /// 状态为了触发准备技能，而发出的sender<br/>
    /// 注意：准备技能的触发要求角色 在前台 活着 未被冻结
    /// </summary>
    public class ActionUsePrepareSender : SimpleSender, ITriggerableIndexSupplier
    {
        public int Character { get; }
        public int Index { get; }
        int ITriggerableIndexSupplier.SourceIndex => Character;
        int ITriggerableIndexSupplier.TriggerableIndex => Index;

        internal ActionUsePrepareSender(int teamID, int character, int index) : base(teamID)
        {
            Character = character;
            Index = index;
        }
    }
}
