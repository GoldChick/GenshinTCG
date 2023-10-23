using TCGGame;
//TODO:check it?
//TODO:应该考虑与persistentset合并
namespace TCGBase
{
    public static class TagsExtendMethod
    {
        public static string ToString(this SenderTags st) => $"minecraft:{st}";
    }
    public delegate void EventPersistentHandler(PlayerTeam me, AbstractPersistent p, AbstractSender? s, AbstractVariable? v);
    public class EventPersistentManager
    {
        public readonly Dictionary<string, TeamPersistentSet> Dic = new();
        public void Register(AbstractPersistent p)
        {
            foreach (var kvp in p.CardBase.TriggerDic)
            {
                if (!Dic.ContainsKey(kvp.Key))
                {
                    Dic.Add(kvp.Key, new TeamPersistentSet());
                }
                //TODO:sender null?
                Dic[kvp.Key].Register(p.PersistentRegion, kvp.Value.Trigger);
            }
        }
        public void Unregister(AbstractPersistent p)
        {
            foreach (var kvp in p.CardBase.TriggerDic)
            {
                Dic[kvp.Key].Unregister(p.PersistentRegion, kvp.Value.Trigger);
            }
        }
        public void Invoke(string sendertag, PlayerTeam me, AbstractSender? s, AbstractVariable? v)
        {
            if (Dic.TryGetValue(sendertag, out var value))
            {
                value.Invoke(me, s, v);
            }
        }
        public class TeamPersistentSet
        {
            private readonly Dictionary<int, EventPersistentHandler?> handlers = new()
            {
                { -1,null},
                { 0,null},
                { 1,null},
                { 2,null},
                { 11,null},
                {12,null}
            };
            public void Register(int region, EventPersistentHandler h) => handlers[region] += h;
            public void Unregister(int region, EventPersistentHandler h) => handlers[region] -= h;
            public void Invoke(PlayerTeam me, AbstractSender? s, AbstractVariable? v)
            {

            }
        }
    }
}
