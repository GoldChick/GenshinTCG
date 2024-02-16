namespace TCGBase
{
    public abstract class AbstractTargetVariable : AbstractVariable
    {
        //给子类用...
        protected int _value;
        public DamageSource Direct { get; private set; }
        public TargetTeam TargetTeam { get; private set; }
        /// <summary>
        /// 目标角色的index，绝对坐标还是相对坐标参见<see cref="TargetRelative"/>
        /// </summary>
        public int TargetIndex { get; private set; }
        /// <summary>
        /// 为true时代表是相对坐标<br/>
        /// 在各种TCGMod中创建的伤害都应该为相对坐标<br/>
        /// 结算伤害时，会转化为绝对坐标
        /// </summary>
        internal bool TargetRelative { get; private set; }
        /// <summary>
        /// 为TargetExcept时，改为对target以外的所有角色造成<br/>
        /// </summary>
        public TargetArea TargetArea { get; init; }
        protected virtual AbstractTargetVariable? Sub { get; }
        protected private AbstractTargetVariable(DamageSource direct, TargetTeam targetTeam, int targetIndex, bool targetRelative, TargetArea targetArea)
        {
            Direct = direct;
            TargetTeam = targetTeam;
            TargetIndex = targetIndex;
            TargetRelative = targetRelative;
            TargetArea = targetArea;
        }
        /// <summary>
        /// 如果是相对坐标，就改成绝对坐标<br/>
        /// me为产生这个东西的队伍
        /// </summary>
        internal void ToAbsoluteIndex(PlayerTeam me)
        {
            if (TargetRelative)
            {
                var team = TargetTeam == TargetTeam.Enemy ? me.Enemy : me;
                if (TargetIndex % team.Characters.Where(c => c.Alive).Count() != 0)
                {
                    do
                    {
                        TargetIndex = (TargetIndex + team.CurrCharacter) % team.Characters.Length;
                    }
                    while (!team.Characters[TargetIndex].Alive);
                }
                else
                {
                    TargetIndex = team.CurrCharacter;
                }
                TargetRelative = false;
                Sub?.ToAbsoluteIndex(team);
            }
        }
    }
}

