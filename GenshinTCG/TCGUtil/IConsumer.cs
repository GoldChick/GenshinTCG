namespace TCGUtil
{
    public interface IConsumer<T>
    {
        public void Accept(T t);
    }
    public interface IBiConsumer<T, U>
    {
        public void Accept(T t, U u);
    }
}
