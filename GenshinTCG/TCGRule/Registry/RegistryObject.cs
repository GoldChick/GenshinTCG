namespace TCGRule
{
    public class RegistryObject<T>
    {
        public string NameID { get; init; }
        public T Value { get; init; }
        public RegistryObject(string nameID, T value)
        {
            NameID = nameID;
            Value = value;
        }
    }
}
