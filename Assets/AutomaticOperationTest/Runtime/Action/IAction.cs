using System;

namespace AutomaticOperationTest.Action
{
    public interface IAction : IDisposable
    {
        public string Name { get; }
        public Priority GetPriority();
        public void Setup(IActionLogger logger);
        public ActionState Execute(IActionLogger logger);
    }

    public enum ActionState
    {
        Running,
        Finished,
    }
}