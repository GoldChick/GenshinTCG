namespace TCGBase
{
    public static class Normalize
    {
        /// <summary>
        /// 将格式不明的Cost转换成int[8]
        /// </summary>
        public static void CostNormalize(int[]? input, out int[] destination)
        {
            input ??= Array.Empty<int>();
            if (input.Length >= 8)
            {
                destination = input[..8];
            }
            else
            {
                destination = new int[8];
                input.CopyTo(destination, 0);
            }
            for (int i = 0; i < 8; i++)
                if (destination[i] < 0)
                    destination[i] = 0;
        }
    }
}
