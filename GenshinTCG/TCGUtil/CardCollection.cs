using System.Text.Json;
using System.Xml.Linq;
using TCGCard;

namespace TCGUtil
{
    internal abstract class CardCollection
    {
        protected string _currModID = "minecraft";
        public void MoveModToNext(string next) => _currModID = Normalize.StringNormalize(next);

    }
    internal class CardCollection<T> : CardCollection, IConsumer<T>, IPrintable where T : ICardBase
    {
        private readonly Dictionary<string, T> _values;

        public CardCollection()
        {
            _values = new();
        }
        public T this[string nameID] { get => _values[nameID]; set => _values[nameID] = value; }

        public void Accept(T t) => Logger.Error($"Registry:注册名为{t.NameID}的{typeof(T)}时出现了问题!",
            !_values.TryAdd(Normalize.NameIDNormalize(t.NameID, _currModID), t));

        public bool ContainsKey(string nameID) => _values.ContainsKey(nameID);
        public bool TryGetValue(string nameID, out T value) => _values.TryGetValue(nameID, out value);
        public T GetValueOrDefault(string nameID) => ContainsKey(nameID) ? _values[nameID] : default(T);

        public void Print()
        {
            var collection = _values.Select(kvp => kvp.Key);

            Logger.Print(JsonSerializer.Serialize(collection, new JsonSerializerOptions() { WriteIndented = true }));
        }
    }
}
