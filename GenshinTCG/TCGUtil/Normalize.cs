using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using TCGBase;

namespace TCGUtil
{
    public static class Normalize
    {
        /// <summary>
        /// 将格式不明的Cost转换成int[8]
        /// </summary>
        public static void CostNormalize(int[] input, out int[] destination)
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
        
        public static string StringNormalize(string? str, [NotNull] string defaultStr = "minecraft")
        => string.IsNullOrEmpty(str) ? defaultStr : Regex.Replace(str, @"[^\w]", "", RegexOptions.None, TimeSpan.FromSeconds(1.5)).ToLower();
        
        public static string NameIDNormalize(string? nameID, [NotNull] string defaultNameSpace = "minecraft")
        {
            nameID ??= "minecraft:keqing";
            string[] strs = Regex.Replace(nameID, @"[^\w\:]", "", RegexOptions.None, TimeSpan.FromSeconds(1.5)).ToLower().Split(':');
            return strs.Length switch
            {
                0 => $"{defaultNameSpace}:keqing",
                1 => $"{defaultNameSpace}:{strs[0]}",
                _ => $"{strs[0]}:{strs[1]}"
            };
        }
    }
}
