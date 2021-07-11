# AutomaticOperationTest

Automatic Operation Test

*Under development*

<a href="https://www.buymeacoffee.com/kyubuns" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/default-orange.png" alt="Buy Me A Coffee" height="41" width="174"></a>

## How to use

```csharp
[MenuItem("Test/AutomaticOperationTest/Run1")]
public static void Run1()
{
    Core.TimeScale = 10.0f;
    var runner = Runner.Run(new IAction[]
    {
        new RandomButtonClickAction(new RandomButtonClickActionOptions
        {
            Condition = Condition.Is<Button>(x =>
                !x.gameObject.GetFullName().StartsWith("AbcConsole(Clone)/") // AbcConsoleのボタンは除外
            )
        })
    }, new RunnerOptions
    {
        LogToConsole = false,
    });

    runner.ErrorDetected += x =>
    {
        var (logger, error) = x;
        Debug.Log($"Error Detected", ("Action", logger.CurrentAction));
        Debug.Log($"{error.Condition}");
        Debug.Log($"{error.StackTrace}");
        foreach (var log in logger.CurrentActionLogs)
        {
            Core.Log($"{log}");
        }
        // スクリーンショットを撮ってSlackに送るとか
    };
}
```

## Installation

### UnityPackageManager

- `https://github.com/kyubuns/AutomaticOperationTest.git?path=Assets/AutomaticOperationTest`

## Target Environment

- Unity2020.3 or later

## License

MIT License (see [LICENSE](LICENSE))

## Buy me a coffee

Are you enjoying save time?  
Buy me a coffee if you love my code!  
https://www.buymeacoffee.com/kyubuns

## "I used it for this game!"

I'd be happy to receive reports like "I used it for this game!"  
Please contact me by email, twitter or any other means.  
(This library is MIT licensed, so reporting is NOT mandatory.)  
[Message Form](https://kyubuns.dev/message.html)

https://kyubuns.dev/

