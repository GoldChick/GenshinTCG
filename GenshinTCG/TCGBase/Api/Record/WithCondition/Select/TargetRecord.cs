using System.Text.Json.Serialization;

namespace TCGBase
{
    /*
     *                     Sender => Persistent
     * AfterUseSkillSender => 使用技能的角色
     * HurtSourceSender   => 作为来源的Persistent+(要)受到伤害的角色
     * DiceModifierSender => 作为费用来源的Persistent
     * OnCharacterOnSender>入场的角色
     */
    public enum TargetType
    {
        Lua,
        //从出战角色开始向右
        Character,
        Summon,
        Support,
        //↓下面对于Select没用↓

        //仅用于添加状态，此时不调用GetTargets()
        Team,
        //仅用于转移状态
        Hand,
    }
}
