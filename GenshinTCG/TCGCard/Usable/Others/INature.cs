namespace TCGCard
{
    /// <summary>
    /// 仅表明为天赋牌，具体的Category还是需要继承其他的类<br/>
    /// 以此实现普通天赋牌和无相之雷天赋牌这种差异
    /// </summary>
    public interface INature
    {
        public string Character { get; }//所属的角色
        
        // @desperated
        // 这种转移到在写技能的地方写判定
        // public ICardSkill Skill { get; }//所强化的技能
    }
}
