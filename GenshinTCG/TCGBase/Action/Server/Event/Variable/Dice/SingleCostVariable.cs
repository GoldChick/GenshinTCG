namespace TCGBase
{
    //TODO: 类似 CostRecord，是否考虑合并一下
    public class SingleCostVariable : AbstractVariable
    {
        private int _count;
        public ElementCategory Element { get; }
        public int Count { get => _count; set => _count = int.Max(0, value); }

        public SingleCostVariable(ElementCategory element, int count)
        {
            Element = element;
            Count = count;
        }
    }
}
