﻿using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Caching
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int ttlSeconds;

        public CachedAttribute(int ttlSeconds)
        {
            this.ttlSeconds = ttlSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<CachedAttribute>>();

            try
            {
                var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
                var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

                await GetOrCacheResponse(context, next, logger, cacheService, cacheKey);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unable to get or cache response in Redis!");

                await next();
            }
        }

        private async Task GetOrCacheResponse(ActionExecutingContext context, ActionExecutionDelegate next, ILogger<CachedAttribute> logger, IResponseCacheService cacheService, string cacheKey)
        {
            if (!HttpMethods.IsGet(context.HttpContext.Request.Method))
            {
                await next();
                return;
            }

            var cachedResponse = await cacheService.GetCachedResponseAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedResponse))
            {
                logger.LogInformation("Returing cached request for {CacheKey}", cacheKey);

                var contentResult = new ContentResult
                {
                    Content = cachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };

                context.Result = contentResult;

                return;
            }

            logger.LogInformation("Response was not in Redis Cache!");

            var result = await next();

            if (result.Result is OkObjectResult okObjectResult)
            {
                logger.LogInformation("Caching request {CacheKey} for {Seconds} seconds in Redis", cacheKey, ttlSeconds);

                await cacheService.CacheResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(ttlSeconds));
            }
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();

            keyBuilder.Append($"{request.Path}");

            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }

            return keyBuilder.ToString();
        }
    }
}
