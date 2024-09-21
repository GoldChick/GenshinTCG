using NLua.Method;

namespace TCGBase
{
    internal class LuaActionHandler : LuaDelegate
    {
        void CallFunction()
        {
            object[] args = Array.Empty<object>();
            object[] inArgs = Array.Empty<object>();
            int[] outArgs = Array.Empty<int>();
            CallFunction(args, inArgs, outArgs);
        }
    }
    internal class LuaActionHandler<T> : LuaDelegate
    {
        T CallFunction()
        {
            object[] args = Array.Empty<object>();
            object[] inArgs = Array.Empty<object>();
            int[] outArgs = Array.Empty<int>();

            return (T)CallFunction(args, inArgs, outArgs);
        }
    }
    internal class LuaPredicateHandler<T> : LuaDelegate
    {
        bool CallFunction(T t)
        {
            if (t == null)
            {
                return false;
            }
            object[] args = new object[] { t };
            object[] inArgs = new object[] { t };
            int[] outArgs = Array.Empty<int>();

            return (bool)CallFunction(args, inArgs, outArgs);
        }
    }
}
