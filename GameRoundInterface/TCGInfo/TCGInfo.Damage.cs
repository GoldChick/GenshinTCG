using TCGBase;

namespace TCGInfo
{
    public struct DamageInfo
    {
        public int DamageValue { get; set; }
        public bool Pierce { get; set; }
        public ElementType ElementType { get; set; }
        public string Reaction { get; set; }
        public string Source { get; set; }
    }
}
