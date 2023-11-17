using System.Collections;

namespace TCGBase
{
    public interface IRegistryConsumer<T> where T : AbstractCardBase
    {
        public void Accept(T t);
        //public void Extend(RegistryCardCollection<T> collection);
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
    public class RegistryCardCollection<T> : RegistryCardCollection, IRegistryConsumer<T>,IEnumerable<KeyValuePair<string, T>> where T : AbstractCardBase
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
        //public void Extend(RegistryCardCollection<T> collection)
        //{
        //    foreach (var item in collection._values)
        //    {
        //        if (!_values.TryAdd(item.Key, item.Value))
        //        {
        //            throw new Exception($"Registry:注册nameID为{item.Key}的{typeof(T)}时出现了问题:nameID被占用!");
        //        }
        //    }
        //}
        public bool ContainsKey(string nameID) => _values.ContainsKey(nameID);
        public bool TryGetValue(string nameID, out T? value) => _values.TryGetValue(nameID, out value);
        public T? GetValueOrDefault(string nameID) => ContainsKey(nameID) ? _values[nameID] : default;

        IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();

        IEnumerator<KeyValuePair<string, T>> IEnumerable<KeyValuePair<string, T>>.GetEnumerator() => _values.GetEnumerator();
    }
}
