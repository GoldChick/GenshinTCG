namespace TCGUtil
{
    public static class Cost
    {
        /// <summary>
        /// 将格式不明的Cost转换成int[8]
        /// </summary>
        public static void NormalizeCost(int[] input, out int[] destination)
        {
            if (input.Length >= 8)
            {
                destination = input[..8];
            }
            else
            {
                destination = new int[8];
                input.CopyTo(destination, 0);
            }
        }
    }
}
