using System.Diagnostics;

namespace TCGUtil
{
    public static class Logger
    {
        //TODO:DEBUG MODE
        private static void Print(string? m, string prefix, bool debug = false)
        {
            Console.WriteLine($"[{DateTime.Now} {prefix}]:{(debug ? "DEBUG:" : null)}{m}");
        }
        private static void Print(string? m, ConsoleColor color, string prefix, bool debug = false)
        {
            Console.ForegroundColor = color;
            Print(m, prefix, debug);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Print(object m, bool debug = false) => Print(m.ToString(), debug);
        public static void Print(string? m, bool debug = false)
        {
            Print(m, "PRINT", debug);
        }
        public static void Print(string? m, ConsoleColor color, bool debug = false)
        {
            Console.ForegroundColor = color;
            Print(m, "PRINT", debug);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void Warning(string? m, bool debug = false) => Print(m, ConsoleColor.Yellow, "WARNING", debug);
        public static void Error(string? m, bool condition = false, bool debug = false)
        {
            if (!condition)
            {
                Print(m, ConsoleColor.Red, "ERROR", debug);
            }
        }
    }
}
