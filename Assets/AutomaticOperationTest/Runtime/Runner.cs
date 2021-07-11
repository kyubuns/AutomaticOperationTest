using System;
using System.Collections.Generic;
using AutomaticOperationTest.Action;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AutomaticOperationTest
{
    public class Runner : IDisposable
    {
        public static Runner Run(IAction[] actions, RunnerOptions options = null)
        {
            if (options == null) options = new RunnerOptions();
            var runner = new Runner(actions, options);
            return runner;
        }

        public IReadOnlyLogger Logger { get; }
        public Action<(IReadOnlyLogger Logger, (string Condition, string StackTrace, LogType Type))> ErrorDetected { get; set; }

        private readonly RunnerOptions _options;
        private readonly GameObject _gameObject;
        private readonly UnityRunner _unityRunner;
        private bool _disposed;

        private Runner(IAction[] actions, RunnerOptions options)
        {
            _options = options;
            _gameObject = new GameObject("AutomaticOperationTestRunner");
            Object.DontDestroyOnLoad(_gameObject);

            var logger = new Logger(_options.LogToConsole);
            Logger = logger;

            _unityRunner = _gameObject.AddComponent<UnityRunner>();
            _unityRunner.Actions = actions;
            _unityRunner.Logger = logger;

            Application.logMessageReceived += HandleLog;
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;
            Application.logMessageReceived -= HandleLog;
            foreach (var action in _unityRunner.Actions)
            {
                action.Dispose();
            }
            Object.Destroy(_gameObject);
        }

        private void HandleLog(string condition, string stacktrace, LogType type)
        {
            if (type == LogType.Log || type == LogType.Warning) return;
            ErrorDetected?.Invoke((Logger, (condition, stacktrace, type)));
            if (_options.StopOnError) Dispose();
        }
    }

    public class UnityRunner : MonoBehaviour
    {
        public IAction[] Actions { get; set; }
        public Logger Logger { get; set; }
        private IAction _runningAction;
        private IActionLogger _runningActionLogger;

        public void Update()
        {
            if (_runningAction == null)
            {
                var random = new List<IAction>();

                foreach (var action in Actions)
                {
                    var state = action.GetPriority();
                    if (state == Priority.Random) random.Add(action);
                }

                if (random.Count > 0)
                {
                    _runningAction = random[UnityEngine.Random.Range(0, random.Count)];
                    _runningActionLogger = Logger.CreateActionLogger(_runningAction);
                    _runningAction.Setup(_runningActionLogger);
                }
            }

            if (_runningAction != null)
            {
                var state = _runningAction.Execute(_runningActionLogger);
                if (state == ActionState.Finished)
                {
                    _runningAction.Dispose();
                    _runningActionLogger.Dispose();
                    _runningAction = null;
                    _runningActionLogger = null;
                }
            }
        }
    }

    public enum Priority
    {
        Random,
        None,
    }

    public class RunnerOptions
    {
        public bool StopOnError { get; set; } = true;
        public bool LogToConsole { get; set; } = true;
    }
}
