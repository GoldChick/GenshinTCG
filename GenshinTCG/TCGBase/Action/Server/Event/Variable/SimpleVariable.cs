namespace TCGBase
{
    public class SimpleVariable<T> : AbstractVariable
    {
        public override string VariableName => Tags.VariableTags.FAST_ACTION;
        public T Fast { get; set; }
        public SimpleVariable(T fast) 
        {
            Fast = fast;
        }
    }
}
