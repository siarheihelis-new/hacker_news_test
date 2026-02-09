using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System.Runtime.CompilerServices;

namespace HackerNews.Data
{
    public static class PollyPolicies
    {
        //TODO: move all constants to configuration
        //Retry configuration constants
        internal const int MaxRetryCount = 3;
        internal const int BaseSleepForRetryInMilliseconds = 100;
        //Circuit breaker configuration constants
        internal const int AllowedEventsBeforeBreaking = 5;
        internal const int DurationOfBreakInSeconds = 10;
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError() // 5xx, 408, HttpRequestException
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt =>
                        TimeSpan.FromMilliseconds(Math.Pow(2, retryAttempt) * BaseSleepForRetryInMilliseconds)
                );
        }

        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: AllowedEventsBeforeBreaking,
                    durationOfBreak: TimeSpan.FromSeconds(DurationOfBreakInSeconds)
                );
        }

        public static IHttpClientBuilder AddResilience(this IHttpClientBuilder clientBuilder)
        {
            clientBuilder.AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());
            return clientBuilder;
        }
    }
}
