using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace TCGBase
{
    public class CardsInHand : AbstractPersistentSet, IEnumerable<Persistent<AbstractCardAction>>
    {
        private readonly List<Persistent<AbstractCardAction>> _data;
        /// <summary>
        /// 其中的卡牌PersistentRegion为-20~-11
        /// </summary>
        public CardsInHand(PlayerTeam me) : base(me)
        {
            PersistentRegion = -20;
            _data = new();
        }
        public Persistent<AbstractCardAction> this[int i] => _data[i];
        internal void Add(AbstractCardAction card)
        {
            if (_data.Count >= 10)
            {
                _me.Game.BroadCast(ClientUpdateCreate.CardUpdate(_me.TeamIndex, ClientUpdateCreate.CardUpdateCategory.Broke, card.Namespace, card.NameID));
            }
            else
            {
                _data.Add(new(card) { PersistentRegion = _data.Count + PersistentRegion });
                _me.Game.BroadCast(ClientUpdateCreate.CardUpdate(_me.TeamIndex, ClientUpdateCreate.CardUpdateCategory.Obtain, card.Namespace, card.NameID));
            }
        }
        internal List<EventPersistentSetHandler> GetHandlers(AbstractSender sender)
        {
            List<EventPersistentSetHandler> sethandlers = new();
            if (sender.TeamID == _me.TeamIndex && sender is ActionUseCardSender cs && cs.Card >= 0 && cs.Card <= _data.Count)
            {
                //这里的s就是上面的cs
                sethandlers.Add((s, v) =>
                {
                    var card = _data[cs.Card];
                    _data.RemoveAt(cs.Card);
                    switch (card.CardBase.CardType)
                    {
                        case CardType.Equipment:
                            if (cs.Persistents.FirstOrDefault(pe => pe is Character) is Character c)
                            {
                                c.AddEffect(card.CardBase);
                            }
                            break;
                        case CardType.Support:
                            _me.AddSupport(new Persistent(card.CardBase), cs.Persistents.FirstOrDefault()?.PersistentRegion ?? -1);
                            break;
                        case CardType.Event:
                            break;
                        default:
                            throw new ArgumentException($"CardsInHand:你把什么东西扔到手里了？{card.CardBase.CardType}?");
                    }
                    foreach (var it in card.CardBase.TriggerableList[cs.SenderName])
                    {
                        it.Trigger(_me, card, s, v);
                    }
                    _me.Game.EffectTrigger(new AfterUseCardSender(_me.TeamIndex, card, cs.Persistents));
                });
            }
            return sethandlers;
        }
        public bool TryPopAt(int index, [NotNullWhen(true)] out Persistent<AbstractCardAction>? persistent)
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
        internal List<Persistent<AbstractCardAction>> Pop(IEnumerable<int> args)
        {
            //args 0 1组成的list，1代表要pop出去
            var cashe = _data.Where((persistent, index) => args.ElementAtOrDefault(index) == 0).ToList();
            var pop = _data.Where((persistent, index) => args.ElementAtOrDefault(index) == 1).ToList();
            _data.Clear();
            _data.AddRange(cashe);
            return pop;
        }
        internal void DestroyAt(int index)
        {
            _data.RemoveAt(index);
            _me.Game.BroadCast(ClientUpdateCreate.CardUpdate(_me.TeamIndex, ClientUpdateCreate.CardUpdateCategory.Blend, index));
        }
        public void TryDestroyAt(int index)
        {
            if (index >= 0 && index < _data.Count)
            {
                _data.RemoveAt(index);
                _me.Game.BroadCast(ClientUpdateCreate.CardUpdate(_me.TeamIndex, ClientUpdateCreate.CardUpdateCategory.Blend, index));
            }
        }
        public int TryDestroyAll(Predicate<Persistent> condition)
        {
            int sum = 0;
            for (int i = _data.Count - 1; i >= 0; i--)
            {
                if (condition.Invoke(_data[i]))
                {
                    DestroyAt(i);
                    sum++;
                }
            }
            return sum;
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
        IEnumerator IEnumerable.GetEnumerator() => _data.GetEnumerator();
        public IEnumerator<Persistent<AbstractCardAction>> GetEnumerator() => _data.GetEnumerator();
    }
}
