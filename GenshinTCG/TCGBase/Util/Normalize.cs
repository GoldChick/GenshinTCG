﻿using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
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

        public static string StringNormalize(string? str, [NotNull] string defaultStr = "minecraft")
        => string.IsNullOrEmpty(str) ? defaultStr : Regex.Replace(str, @"[^\w]", "", RegexOptions.None, TimeSpan.FromSeconds(1.5)).ToLower();
    }
}
