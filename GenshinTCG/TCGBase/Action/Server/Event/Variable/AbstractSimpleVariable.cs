namespace TCGBase
{
    public abstract class AbstractSimpleVariable<T> : AbstractVariable
    {
        public T Param { get; set; }
        public AbstractSimpleVariable(T param) 
        {
            Param = param;
        }
    }
}
