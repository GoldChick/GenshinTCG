namespace TCGBase
{
    public class DamageVariable : AbstractVariable
    {
        public override string VariableName => Tags.VariableTags.DAMAGE;
        /// <summary>
        /// 物理 冰水火雷岩草风
        /// </summary>
        public int Element { get; set; }
        public int Damage { get; set; }
        /// <summary>
        /// 穿透伤害
        /// </summary>
        public bool Impale { get; set; }
        //TODO:Check it, Source Not Clear
        public int Source { get; set; }
        public int Target { get; set; }
        public DamageVariable(int element, int damage, bool impale)
        {
            Element = element;
            Damage = damage;
            Impale = impale;
        }
    }
}
