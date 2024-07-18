namespace TCGBase
{
    /// <summary>
    /// 用全局计数，如婕德 
    /// </summary>
    public class ListenerManager
    {
        private readonly PlayerTeam _me;
        private readonly Dictionary<string, Persistent> _data;
        private readonly Dictionary<string, EventPersistentSetHandler?> _handlers;
        public List<int> this[string registryName] => _data.TryGetValue(registryName, out var value) ? value.Data : new List<int>();
        public ListenerManager(PlayerTeam me)
        {
            _me = me;
            _data = new();
            _handlers = new();
            foreach (var kvp in Registry.Instance.ListenerCards)
            {
                _data[kvp.Key] = new(kvp.Value);
                foreach (var triggerable in kvp.Value.TriggerableList)
                {
                    if (!_handlers.ContainsKey(triggerable.Tag))
                    {
                        _handlers[triggerable.Tag] = null;
                    }
                    _handlers[triggerable.Tag] += (s, v) => triggerable.Trigger(_me, _data[kvp.Key], s, v);
                }
            }
        }
        internal EventPersistentSetHandler? GetHandlers(string sendertag)
        {
            return _handlers.TryGetValue(sendertag, out var value) ? value : null;
        }
    }
}
