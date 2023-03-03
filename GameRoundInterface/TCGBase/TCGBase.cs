namespace TCGBase
{
    /// <summary>
    /// 元素类型(实际上并不仅仅是元素)
    /// 0为Trival，对应物理攻击/万能元素骰
    /// 1-7为七元素
    /// </summary>
    public enum ElementType
    {
        Trival,
        Anemo,
        Geo,
        Electro,
        Dendro,
        Hydro,
        Pyro,
        Cryo
    }
    /// <summary>
    /// 某次行动的type
    /// </summary>
    public enum ActionType
    {
        None,//什么也不做
        Pass,//空过
        Blend,//调和
        Switch,
        UseAssistCard,

        UseSkill,
        Hurt,

        GainDice,
        GainCard,

        Die,

        Others//虽然但是，似乎没有什么其他行动了
    }
}
