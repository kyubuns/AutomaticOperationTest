using System;
using System.Collections.Generic;
using System.Linq;
using AutomaticOperationTest.Action;
using UnityEngine;

namespace AutomaticOperationTest
{
    public interface IReadOnlyLogger
    {
        public LogLine[] Logs { get; }
        public LogLine[] CurrentActionLogs { get; }
        public IAction CurrentAction { get; }
    }

    public interface IActionLogger : IDisposable
    {
        public void Log(string message);
        public void Log(string message, params (string, object)[] values);
    }

    public class Logger : IReadOnlyLogger
    {
        public LogLine[] Logs => _logs.ToArray();
        public LogLine[] CurrentActionLogs => _current?.Logs?.ToArray() ?? new LogLine[] { };
        public IAction CurrentAction => _current?.Action;

        private readonly bool _console;
        private readonly List<LogLine> _logs = new List<LogLine>();
        private ActionLogger _current;

        public Logger(bool console)
        {
            _console = console;
        }

        public IActionLogger CreateActionLogger(IAction action)
        {
            _current = new ActionLogger(this, action);
            return _current;
        }

        private void Log(LogLine logLine)
        {
            _logs.Add(logLine);
            _current?.Logs.Add(logLine);
            if (_console) Debug.Log(logLine.ToString());
        }

        public void Log(string message)
        {
            Log(new LogLine(Time.frameCount, Time.unscaledTime, message, null));
        }

        public void Log(string message, params (string, object)[] values)
        {
            Log(new LogLine(Time.frameCount, Time.unscaledTime, message, values.Select(x => (x.Item1, x.Item2.ToString())).ToArray()));
        }

        private class ActionLogger : IActionLogger
        {
            public IAction Action { get; }

            private readonly Logger _logger;
            public readonly List<LogLine> Logs = new List<LogLine>();

            public ActionLogger(Logger logger, IAction action)
            {
                Action = action;
                _logger = logger;
            }

            public void Dispose()
            {
                _logger._current = null;
            }

            public void Log(string message)
            {
                _logger.Log(message);
            }

            public void Log(string message, params (string, object)[] values)
            {
                _logger.Log(message, values);
            }
        }
    }

    public class LogLine
    {
        public int Frame { get; }
        public float Time { get; }
        public string Message { get; }
        public (string, string)[] Values { get; }

        public LogLine(int frame, float time, string message, (string, string)[] values)
        {
            Frame = frame;
            Time = time;
            Message = message;
            Values = values;
        }

        public override string ToString()
        {
            if (Values == null)
            {
                return $"{Frame} ({Time:0.00}s) {Message}";
            }
            else
            {
                return $"{Frame} ({Time:0.00}s) {Message} | {string.Join(", ", Values.Select(x => $"{x.Item1} = {x.Item2}"))}";
            }
        }
    }
}
