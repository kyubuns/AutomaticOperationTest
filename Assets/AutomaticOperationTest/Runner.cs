using System;
using System.Collections.Generic;
using AutomaticOperationTest.Action;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AutomaticOperationTest
{
    public class Runner : IDisposable
    {
        public static Runner Run(IAction[] actions)
        {
            var runner = new Runner(actions);
            return runner;
        }

        public IReadOnlyLogger Logger { get; }
        public Action<(string condition, string stackTrace, LogType type)> ErrorDetected { get; set; }

        private bool _disposed;
        private readonly GameObject _gameObject;

        private Runner(IAction[] actions)
        {
            _gameObject = new GameObject("AutomaticOperationTestRunner");

            var logger = new Logger();
            Logger = logger;

            var unityRunner = _gameObject.AddComponent<UnityRunner>();
            unityRunner.Actions = actions;
            unityRunner.Logger = logger;

            Application.logMessageReceived += HandleLog;
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;
            Application.logMessageReceived -= HandleLog;
            Object.Destroy(_gameObject);
        }

        private void HandleLog(string condition, string stacktrace, LogType type)
        {
            if (type == LogType.Log || type == LogType.Warning) return;
            ErrorDetected?.Invoke((condition, stacktrace, type));
        }
    }

    public class UnityRunner : MonoBehaviour
    {
        public IAction[] Actions { get; set; }
        public Logger Logger { get; set; }

        public void Update()
        {
            var must = new List<IAction>();
            var random = new List<IAction>();

            foreach (var action in Actions)
            {
                var state = action.GetPriority();
                if (state == Priority.Must) must.Add(action);
                if (state == Priority.Random) random.Add(action);
            }

            if (must.Count > 0)
            {
                foreach (var m in must)
                {
                    m.Execute(Logger.CreateActionLogger(m));
                }
            }
            else if (random.Count > 0)
            {
                var randomPick = random[UnityEngine.Random.Range(0, random.Count)];
                randomPick.Execute(Logger.CreateActionLogger(randomPick));
            }
        }

        public void OnDestroy()
        {
            foreach (var action in Actions)
            {
                action.Dispose();
            }
        }
    }

    public enum Priority
    {
        Must,
        Random,
        None,
    }
}
