using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata.Ecma335;

namespace TCGBase
{
    public class CardsInHand : AbstractPersistentSet, IEnumerable<Persistent>
    {
        private readonly List<Persistent> _data;
        public CardsInHand()
        {
            PersistentRegion = -10;
            _data = new();
        }
        public Persistent this[int i] => _data[i];
        internal void Add(AbstractCardAction card)
        {
            //TODO: broadcast
            if (_data.Count >= 10)
            {
                //RealGame.BroadCast(ClientUpdateCreate.CardUpdate(TeamIndex, ClientUpdateCreate.CardUpdateCategory.Broke, card.Namespace, card.NameID));
            }
            else
            {
                _data.Add(new(card));
                //RealGame.BroadCast(ClientUpdateCreate.CardUpdate(TeamIndex, ClientUpdateCreate.CardUpdateCategory.Obtain, card.Namespace, card.NameID));
            }
        }
        internal EventPersistentSetHandler? GetHandler(AbstractSender sender)
        {
            if (sender is ActionUseCardSender cs && cs.Card >= 0 && cs.Card <= _data.Count)
            {
                EventPersistentHandler? handler = null;
                var card = _data[cs.Card];
                foreach (var it in card.CardBase.TriggerableList[cs.SenderName])
                {
                    handler += it.Trigger;
                }
                return Persistent.GetDelayedHandler((me, s, v) =>
                {
                    _data.RemoveAt(cs.Card);
                    switch (card.CardBase.CardType)
                    {
                        case CardType.Equipment:
                            break;
                        case CardType.Support:
                            break;
                        case CardType.Event:
                            break;
                        default:
                            throw new ArgumentException($"CardsInHand:你把什么东西扔到手里了？{card.CardBase.CardType}?");
                    }
                    //TODO:依据种类的不同而xx；装备牌装到身上；支援牌放到支援区；事件牌直接消失
                    handler?.Invoke(me, card, s, v);
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
