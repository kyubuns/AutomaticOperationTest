using UnityEngine;

namespace AutomaticOperationTest.Action
{
    public class RandomWaitAction : IAction
    {
        public string Name => nameof(RandomWaitAction);

        private readonly RandomWaitActionOptions _options;
        private float _remaining;

        public RandomWaitAction(RandomWaitActionOptions options = null)
        {
            _options = options ?? new RandomWaitActionOptions();
        }

        public void Dispose()
        {
        }

        public Priority GetPriority()
        {
            return Priority.Random;
        }

        public void Setup(IActionLogger logger)
        {
            _remaining = Random.Range(_options.Min, _options.Max);
        }

        public ActionState Execute(IActionLogger logger)
        {
            _remaining -= Time.deltaTime;
            if (_remaining < 0) return ActionState.Finished;
            return ActionState.Running;
        }
    }

    public class RandomWaitActionOptions
    {
        public float Min { get; set; } = 0f;
        public float Max { get; set; } = 1f;
    }
}