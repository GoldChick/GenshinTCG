namespace TCGInfo
{
    /// <summary>
    /// 不会改变的信息
    /// 只请求一次，实际上是一种简单优化的行为(?这种低能东西真的需要优化吗?)
    /// </summary>
    public class CardReadonlyInfo
    {
        public CardReadonlyInfo(string iD, string tags)
        {
            ID = iD;
            Tags = tags;
        }

        public string ID { get; }
        public string Tags { get; }
    }
    public class CharacterReadonlyInfo : CardReadonlyInfo
    {
        public CharacterReadonlyInfo(string iD, string tags, int maxMp, string[] skills) : base(iD, tags)
        {
            MaxMp = maxMp;
            Skills = skills;
        }
        public int MaxMp { get; }
        public string[] Skills { get; set; }
    }
}
