using System.Collections;

namespace TCGBase
{
    public interface IRegistryConsumer<T> where T : AbstractCardBase
    {
        public void Accept(T t);
        public void AcceptMulti(params T[] ts) => Array.ForEach(ts, t => Accept(t));
    }
    public class RegistryCardCollection<T> : IRegistryConsumer<T>, IEnumerable<KeyValuePair<string, T>> where T : AbstractCardBase
    {
        private readonly Dictionary<string, T> _values;

        public RegistryCardCollection()
        {
            _values = new();
        }
        public T this[string nameID] { get => _values[nameID]; set => _values[nameID] = value; }

        public void Accept(T t)
        {
            if (!_values.TryAdd($"{t.Namespace}:{t.NameID}", t))
            {
                throw new Exception($"Registry:Mod {t.Namespace}注册名为{t.NameID}的{typeof(T)}时出现了问题:nameID被占用!");
            }
        }
        public bool ContainsKey(string nameID) => _values.ContainsKey(nameID);
        public bool TryGetValue(string nameID, out T? value) => _values.TryGetValue(nameID, out value);
        public T? GetValueOrDefault(string nameID) => ContainsKey(nameID) ? _values[nameID] : default;
        IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();
        IEnumerator<KeyValuePair<string, T>> IEnumerable<KeyValuePair<string, T>>.GetEnumerator() => _values.GetEnumerator();
    }
}
