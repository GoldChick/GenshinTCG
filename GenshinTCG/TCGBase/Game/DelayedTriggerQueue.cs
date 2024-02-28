namespace TCGBase
{
    internal class DelayedTriggerQueue
    {
        private Queue<Action> _queue;
        public bool Active { get; set; }
        public DelayedTriggerQueue()
        {
            _queue = new();
        }
        public void TryTrigger(Action action)
        {
            if (Active)
            {
                _queue.Enqueue(action);
            }
            else
            {
                action.Invoke();
            }
        }
        public void TryEmpty()
        {
            Active = false;
            var q = _queue;
            _queue = new();
            while (q.TryDequeue(out var action))
            {
                action.Invoke();
            }
        }
    }
}
