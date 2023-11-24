namespace TCGBase
{
    /// <summary>
    /// 实现妮露之类的异化反应
    /// </summary>
    public class ElementGenerateVariable : AbstractVariable
    {
        /// <summary>
        /// 是否覆写最初的产生
        /// </summary>
        public bool OverrideInitialGenerator { get; set; }
        /// <summary>
        /// playerteam：受到反应的队伍<br/>
        /// int: 角色的index<br/>
        /// </summary>
        public Action<PlayerTeam, int>? GenerateAction { get; set; }


        internal ElementGenerateVariable(bool overrideInitialGenerator, Action<PlayerTeam, int>? generateAction)
        {
            OverrideInitialGenerator = overrideInitialGenerator;
            GenerateAction = generateAction;
        }
    }
}
