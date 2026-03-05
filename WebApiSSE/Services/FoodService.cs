using System.Runtime.CompilerServices;

namespace WebApiSSE.Services;

public class FoodService
{
    private static readonly string[] Foods = ["🍔", "🍟", "🥤", "🍤", "🍕", "🌮", "🥙"];

    public async IAsyncEnumerable<string> GetCurrent([EnumeratorCancellation] CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            yield return Foods[Random.Shared.Next(Foods.Length)];
            await Task.Delay(1000, ct);
        }
    }
}

