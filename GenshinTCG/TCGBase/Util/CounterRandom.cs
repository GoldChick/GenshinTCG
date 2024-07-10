namespace TCGBase
{
    /// <summary>
    /// 调用时计数的random，依次复制random来实现预览
    /// </summary>
    public class CounterRandom
    {
        private Random _random;
        private int _seed;
        private int _counter;
        public CounterRandom()
        {
            Random r = new();
            _seed = r.Next();
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
        /// <returns>0~maxValue-1</returns>
        public int Next(int maxValue) => Next(0, maxValue);
        /// <returns>minValue~maxValue-1</returns>
        public int Next(int minValue, int maxValue)
        {
            _counter++;
            return _random.Next(minValue, maxValue);
        }
        /// <summary>
        /// 返回count个彼此不同的minValue~maxValue-1之间的数。
        /// </summary>
        public List<int> NextNDifferent(int minValue, int maxValue, int count = 1)
        {
            List<int> result = new();
            while (result.Count < count)
            {
                var curr = _random.Next(minValue, maxValue);
                _counter++;
                if (!result.Contains(curr))
                {
                    result.Add(curr);
                }
            }
            return result;
        }
    }
}
