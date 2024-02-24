using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace TCGBase
{
    public class CardsInHand : AbstractPersistentSet, IEnumerable<Persistent>
    {
        private readonly List<Persistent> _data;
        private readonly PlayerTeam _t;
        public CardsInHand(PlayerTeam t)
        {
            PersistentRegion = -10;
            _data = new();
            _t = t;
        }
        public Persistent this[int i] => _data[i];
        internal void Add(AbstractCardAction card)
        {
            if (_data.Count >= 10)
            {
                _t.Game.BroadCast(ClientUpdateCreate.CardUpdate(_t.TeamIndex, ClientUpdateCreate.CardUpdateCategory.Broke, card.Namespace, card.NameID));
            }
            else
            {
                _data.Add(new(card));
                _t.Game.BroadCast(ClientUpdateCreate.CardUpdate(_t.TeamIndex, ClientUpdateCreate.CardUpdateCategory.Obtain, card.Namespace, card.NameID));
            }
        }
        internal EventPersistentSetHandler? GetHandlers(AbstractSender sender)
        {
            if (sender.TeamID == _t.TeamIndex && sender is ActionUseCardSender cs && cs.Card >= 0 && cs.Card <= _data.Count)
            {
                EventPersistentHandler? handler = null;
                var card = _data[cs.Card];
                foreach (var it in card.CardBase.TriggerableList[cs.SenderName])
                {
                    handler += it.Trigger;
                }
                //这里的s就是上面的cs
                return Persistent.GetDelayedHandler((me, s, v) =>
                {
                    _data.RemoveAt(cs.Card);
                    switch (card.CardBase.CardType)
                    {
                        case CardType.Equipment:
                            if (cs.Persistents.FirstOrDefault() is Character c)
                            {
                                c.AddEffect(card.CardBase);
                            }
                            break;
                        case CardType.Support:
                            me.AddSupport(card.CardBase, cs.Persistents.FirstOrDefault()?.PersistentRegion ?? -1);
                            break;
                        case CardType.Event:
                            break;
                        default:
                            throw new ArgumentException($"CardsInHand:你把什么东西扔到手里了？{card.CardBase.CardType}?");
                    }
                    handler?.Invoke(me, card, s, v);
                    me.Game.EffectTrigger(new AfterUseCardSender(me.TeamIndex, card, cs.Persistents));
                });
            }
            return null;
        }
        public bool TryPopAt(int index, [NotNullWhen(true)] out Persistent? persistent)
        {
            if (index >= 0 && index < _data.Count)
            {
                persistent = _data[index];
                _data.RemoveAt(index);
                return true;
            }
            persistent = null;
            return false;
        }
        internal void DestroyAt(int index)
        {
            _data.RemoveAt(index);
            //RealGame.BroadCast(ClientUpdateCreate.CardUpdate(TeamIndex, ClientUpdateCreate.CardUpdateCategory.Blend, index));
        }
        public void TryDestroyAt(int index)
        {
            if (index >= 0 && index < _data.Count)
            {
                _data.RemoveAt(index);
                //RealGame.BroadCast(ClientUpdateCreate.CardUpdate(TeamIndex, ClientUpdateCreate.CardUpdateCategory.Blend, index));
            }
        }
        public int TryDestroyAll(Predicate<AbstractCardBase> condition)
        {
            int sum = 0;
            for (int i = _data.Count - 1; i >= 0; i--)
            {
                if (condition.Invoke(_data[i].CardBase))
                {
                    DestroyAt(i);
                    sum++;
                }
            }
            return sum;
        }
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_data).GetEnumerator();
        public IEnumerator<Persistent> GetEnumerator() => ((IEnumerable<Persistent>)_data).GetEnumerator();
    }
}
