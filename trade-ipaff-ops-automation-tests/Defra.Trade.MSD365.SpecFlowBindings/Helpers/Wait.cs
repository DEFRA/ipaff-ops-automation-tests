namespace Defra.Trade.Plants.SpecFlowBindings.Helpers;

using System;
using System.Diagnostics;
using System.Threading;

public class Wait
{
    public static void Until(TimeSpan timeSpan, Func<bool> condition, Action action = null)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var outcome = false;

        do
        {
            try
            {
                if (action != null)
                {
                    action.Invoke();
                }

                outcome = condition();
            }
            catch
            {
                // Swallow exception
            }

            if (!outcome)
            {
                if (stopwatch.Elapsed >= timeSpan)
                {
                    throw new TimeoutException($"Timed out after {timeSpan.Minutes}m {timeSpan.Seconds}s");
                }
                else
                {
                    Thread.Sleep(TimeSpan.FromSeconds(0.5));
                }
            }
        }
        while (!outcome);
    }
}
