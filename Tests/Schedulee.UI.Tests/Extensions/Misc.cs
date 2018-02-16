using System;
using Polly;

namespace Schedulee.UI.Tests.Extensions
{
    public static class Misc
    {
        public static void Retry(Action action, int count = 3, int waitMiliseconds = 200)
        {
            Policy.Handle<Exception>()
                  .WaitAndRetry(count, _ => TimeSpan.FromSeconds(waitMiliseconds), (exception, span) => { action(); });
        }
    }
}