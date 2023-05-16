using System;
using Dapr;
using Polly;
using Polly.Retry;

namespace Wemogy.Configuration
{
    public static class RetryPolicies
    {
        public static RetryPolicy ResilientDaprHostBuilder = Policy
            .Handle<DaprException>()
            .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt));
    }

}
