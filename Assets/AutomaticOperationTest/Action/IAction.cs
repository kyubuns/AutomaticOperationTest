using System;

namespace AutomaticOperationTest.Action
{
    public interface IAction : IDisposable
    {
        public string Name { get; }
        public Priority GetPriority();
        public void Execute(IActionLogger logger);
    }
}