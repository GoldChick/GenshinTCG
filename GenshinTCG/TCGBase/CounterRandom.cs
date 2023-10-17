namespace TCGBase
{
    /// <summary>
    /// 调用时计数的random，依次复制random来实现预览
    /// </summary>
    internal class CounterRandom
    {
        private Random _random;
        private int _seed;
        private int _counter;
        public CounterRandom()
        {
            Random r = new();
            _seed=r.Next();
            _random = new(_seed);
        }
        public CounterRandom(int seed)
        {
            _random = new(seed);
            _seed = seed;
        }
        public CounterRandom(CounterRandom cr) : this(cr._seed)
        {
            for (int i = 0; i < _counter; i++)
            {
                _random.Next(10);
            }
            _counter = cr._counter;
        }
        /// <returns>0-maxValue-1</returns>
        public int Next(int maxValue) => _random.Next(0, maxValue);
        public int Next(int minValue, int maxValue)
        {
            _counter++;
            return _random.Next(minValue, maxValue);
        }
    }
}
