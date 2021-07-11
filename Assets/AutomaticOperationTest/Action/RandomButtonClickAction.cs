using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AutomaticOperationTest.Action
{
    public class RandomButtonClickAction : IAction
    {
        public string Name => nameof(RandomButtonClickAction);

        private readonly RandomButtonClickActionOptions _options;
        private Button[] _targetButtons;

        public RandomButtonClickAction(RandomButtonClickActionOptions options = null)
        {
            _options = options ?? new RandomButtonClickActionOptions();
        }

        public void Dispose()
        {
        }

        public Priority GetPriority()
        {
            var buttons = Object.FindObjectsOfType<Button>();
            _targetButtons = buttons
                .Where(x => x.CheckClickable())
                .Where(x => _options.Condition.Is(x))
                .ToArray();
            if (_targetButtons.Length == 0) return Priority.None;
            return Priority.Random;
        }

        public void Execute(IActionLogger logger)
        {
            var targetButton = _targetButtons[Random.Range(0, _targetButtons.Length)].gameObject;
            logger.Log("Click", ("Target", targetButton.GetFullName()));
            ExecuteEvents.Execute
            (
                targetButton,
                new PointerEventData(EventSystem.current),
                ExecuteEvents.pointerClickHandler
            );
        }
    }

    public class RandomButtonClickActionOptions
    {
        public Condition<Button> Condition { get; set; } = AutomaticOperationTest.Condition.None<Button>();
    }
}
