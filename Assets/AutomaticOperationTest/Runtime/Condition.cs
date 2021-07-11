using System;

namespace AutomaticOperationTest
{
    public static class Condition
    {
        public static Condition<T> None<T>() => new Condition<T>(x => true);
        public static Condition<T> Is<T>(Func<T, bool> func) => new Condition<T>(func);
    }

    public class Condition<T>
    {
        private readonly Func<T, bool> _func;

        public Condition(Func<T, bool> func)
        {
            _func = func;
        }

        public bool Is(T t)
        {
            return _func(t);
        }
    }
}