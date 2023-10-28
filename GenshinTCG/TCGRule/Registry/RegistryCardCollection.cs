using System.Text.Json;
using TCGBase;
using TCGRule;

namespace TCGUtil
{
    public interface IRegistryConsumer<T> where T : AbstractCardBase
    {
        public RegistryObject<T> Accept(T t);
        public void Extend(RegistryCardCollection<T> collection);
        public void AcceptMulti(params T[] ts) => Array.ForEach(ts, t => Accept(t));
    }
    public abstract class RegistryCardCollection
    {
        /// <summary>
        /// ONLY FOR REGISTRY
        /// </summary>
        protected string _currModID = "minecraft";
        internal void MoveModToNext(string next) => _currModID = Normalize.StringNormalize(next);

    }
    public class RegistryCardCollection<T> : RegistryCardCollection, IRegistryConsumer<T> where T : AbstractCardBase
    {
        private readonly Dictionary<string, T> _values;

        public RegistryCardCollection()
        {
            _values = new();
        }
        public T this[string nameID] { get => _values[nameID]; set => _values[nameID] = value; }

        public RegistryObject<T> Accept(T t)
        {
            string final_nameID = Normalize.NameIDNormalize(t.NameID, _currModID);
            Logger.Error($"Registry:注册名为{t.NameID}的{typeof(T)}时出现了问题:nameID被占用!", !_values.TryAdd(final_nameID, t));
            return new RegistryObject<T>(final_nameID, t);
        }
        public void Extend(RegistryCardCollection<T> collection)
        {
            foreach (var item in collection._values)
            {
                Logger.Error($"Registry:注册nameID为{item.Key}的{typeof(T)}时出现了问题:nameID被占用!", !_values.TryAdd(item.Key, item.Value));
            }
        }
        public bool ContainsKey(string nameID) => _values.ContainsKey(nameID);
        public bool TryGetValue(string nameID, out T? value) => _values.TryGetValue(nameID, out value);
        public T? GetValueOrDefault(string nameID) => ContainsKey(nameID) ? _values[nameID] : default;

        public void Print()
        {
            var collection = _values.Select(kvp => kvp.Key);

            Logger.Print(JsonSerializer.Serialize(collection, new JsonSerializerOptions() { WriteIndented = true }));
        }
    }
}
