using System;
using AutomaticOperationTest;
using AutomaticOperationTest.Action;
using UnityEngine;
using UnityEngine.UI;

public class SampleScene : MonoBehaviour
{
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Text leftText;
    [SerializeField] private Text rightText;

    [SerializeField] private Button testStartButton;

    public void Start()
    {
        var leftValue = 0;
        leftButton.onClick.AddListener(() =>
        {
            leftValue++;
            leftText.text = leftValue.ToString();

            if (leftValue == 100) throw new Exception($"LeftValue >= 100");
        });

        var rightValue = 0;
        rightButton.onClick.AddListener(() =>
        {
            rightValue++;
            rightText.text = rightValue.ToString();
        });

        testStartButton.onClick.AddListener(() =>
        {
            Destroy(testStartButton.gameObject);

            var runner = Runner.Run(new IAction[]
            {
                new RandomButtonClickAction(new RandomButtonClickActionOptions
                {
                    Condition = Condition.Is<Button>(go => go.name != "Debug")
                })
            });

            runner.ErrorDetected += (x) =>
            {
                var (logger, error) = x;
                Debug.Log("=== Error Detected ===");
                Debug.Log($"{error.Condition}");
                Debug.Log($"Current Action: {logger.CurrentAction.Name}");
                foreach (var log in logger.CurrentActionLogs)
                {
                    Debug.Log(log.ToString());
                }
            };
        });
    }
}
