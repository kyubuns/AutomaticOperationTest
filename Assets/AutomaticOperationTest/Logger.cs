using System;
using System.Linq;
using AutomaticOperationTest.Action;

namespace AutomaticOperationTest
{
    public interface IReadOnlyLogger
    {
    }

    public interface IActionLogger
    {
        public void Log(string message);
        public void Log(string message, params (string, object)[] values);
        public IDisposable LogStart(string message);
        public IDisposable LogStart(string message, params (string, object)[] values);
    }

    public class Logger : IReadOnlyLogger
    {
        public IActionLogger CreateActionLogger(IAction action)
        {
            return new ActionLogger();
        }

        private class ActionLogger : IActionLogger
        {
            public void Log(string message)
            {
                UnityEngine.Debug.Log($"{message}");
            }

            public void Log(string message, params (string, object)[] values)
            {
                UnityEngine.Debug.Log($"{message} | {string.Join(", ", values.Select(x => $"{x.Item1} = {x.Item2}"))}");
            }

            public IDisposable LogStart(string message)
            {
                UnityEngine.Debug.Log($"{message} Start");
                return new DisposableLog($"{message} Finish");
            }

            public IDisposable LogStart(string message, params (string, object)[] values)
            {
                UnityEngine.Debug.Log($"{message} Start | {string.Join(", ", values.Select(x => $"{x.Item1} = {x.Item2}"))}");
                return new DisposableLog($"{message} Finish | {string.Join(", ", values.Select(x => $"{x.Item1} = {x.Item2}"))}");
            }
        }

        private class DisposableLog : IDisposable
        {
            private readonly string _message;

            public DisposableLog(string message)
            {
                _message = message;
            }

            public void Dispose()
            {
                UnityEngine.Debug.Log(_message);
            }
        }
    }
}
