using System.Text.Json;

namespace TCGBase
{
    public record TriggerableRecordCustom : TriggerableRecordBase
    {
        public string Name { get; }
        /// <summary>
        /// 如果Value不为Null，将会通过Value作为json，按照通过Name找到的类型创建一个对象（所以这种情况要有构造函数）
        /// </summary>
        public object? Value { get; }
        public TriggerableRecordCustom(string name, object? value = null) : base(TriggerableType.Custom, null)
        {
            Name = name;
            Value = value;
        }
        public override AbstractTriggerable GetTriggerable()
        {
            if (Registry.Instance.CustomTriggerable.TryGetValue(Name, out var value))
            {
                try
                {
                    if (Value != null)
                    {
                        if (JsonSerializer.Deserialize(Value.ToString() ?? "{}", value.GetType(), RegistryFromJson.JsonOptionGlobal) is AbstractTriggerable obj)
                        {
                            return obj;
                        }
                        else
                        {
                            throw new ArgumentException($"TriggerableRecordCustom: Deserialize \"{Name}\" from json failed to get {nameof(AbstractTriggerable)}.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"TriggerableRecordCustom: Got Error When Deserialize \"{Name}\" from json: \n \"{Value}\" \nMessage:{ex.Message}");
                }
                return value;
            }
            else
            {
                throw new NotImplementedException($"TriggerableRecordCustom: No Such Triggerable \"{Name}\"");
            }
        }
    }
}
