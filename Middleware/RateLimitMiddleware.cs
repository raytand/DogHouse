using Microsoft.Extensions.Caching.Memory;

namespace DogHouse.Api.Middleware
{
    public class RateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly int _limit;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _window = TimeSpan.FromSeconds(1);

        public RateLimitMiddleware(RequestDelegate next, IConfiguration config, IMemoryCache cache)
        {
            _next = next;
            _cache = cache;
            if (!int.TryParse(config["RateLimit:RequestsPerSecond"], out _limit))
                _limit = 10;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var key = "GlobalRateLimit";
            var now = DateTime.UtcNow;

            var counter = _cache.GetOrCreate(key, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _window;
                return new RateCounter { Timestamp = now, Count = 0 };
            });

            lock (counter) 
            {
                counter.Count++;
                if (counter.Count > _limit)
                {
                    context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    return;
                }
            }

            await _next(context);
        }

        private class RateCounter
        {
            public DateTime Timestamp { get; set; }
            public int Count { get; set; }
        }
    }
}