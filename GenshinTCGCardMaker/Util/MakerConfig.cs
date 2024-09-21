namespace GenshinTCGCardMaker
{
    public record MakerConfig
    {
        public string Date { get; set; }
        public int Version { get; set; }
        public string Author { get; set; }
        public string NameSpace { get; set; }
        public List<string> CharacterList { get; set; }
        public List<string> ActionCardList { get; set; }
        public List<string> SkillList { get; set; }
        public MakerConfig(string author = "Gold_Chick", string nameSpace = "minecraft", string? date = null, int version = -1)
        {
            Date = DateTime.Now.ToString();
            Version = version + 1;
            Author = author;
            NameSpace = nameSpace;
        }
    }
}
