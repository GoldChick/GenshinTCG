namespace TCGBase
{
    public record class ActionRecordEatDice : ActionRecordBaseWithTeam
    {
        /// <summary>
        /// 如果为false，就吐出来
        /// </summary>
        public bool Eat { get; }
        /// <summary>
        /// 如果为true，一次可以吃多种元素
        /// </summary>
        public bool Same { get; }
        /// <summary>
        /// 最多能吃几个（注：限制为Data中的数量，不是去一次吃）
        /// </summary>
        public int Count { get; }
        public ActionRecordEatDice(int count, bool same = true, bool eat = true, TargetTeam team = TargetTeam.Me, List<ConditionRecordBase>? when = null) : base(TriggerType.EatDice, team, when)
        {
            Eat = eat;
            Same = same;
            Count = count;
        }
        protected override void DoAction(AbstractTriggerable triggerable, PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v)
        {
            var team = Team == TargetTeam.Enemy ? me.Enemy : me;
            if (Eat)
            {
                int need = Count - p.Data.Count;
                var dices = new Queue<(int count, int element)>(team.GetSortedDices());
                while (need > 0 && dices.TryDequeue(out var pair) && pair.count > 0)
                {
                    if (Same)
                    {
                        while (need > 0 && pair.count > 0)
                        {
                            p.Data.Add(pair.element);
                            team.TryRemoveDice(pair.element);
                            pair.count--;
                            need--;
                        }
                    }
                    else
                    {
                        p.Data.Add(pair.element);
                        team.TryRemoveDice(pair.element);
                        need--;
                    }
                }
            }
            else
            {
                foreach (var element in p.Data)
                {
                    team.AddSingleDice(element);
                }
                p.Data.Clear();
            }
        }
    }
}
