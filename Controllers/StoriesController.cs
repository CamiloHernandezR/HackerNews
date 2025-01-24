using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using HackerNews.Model;

[ApiController]
[Route("api/[controller]")]
public class BestStoriesController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _cache;
    private readonly HackerApiSettings _settings;

    public BestStoriesController(IHttpClientFactory httpClientFactory, IMemoryCache cache, IOptions<HackerApiSettings> settings)
    {
        _httpClientFactory = httpClientFactory;
        _cache = cache;
        _settings = settings.Value;
    }

    [HttpGet]
    public async Task<IActionResult> GetTopStories([FromQuery] int n = 10)
    {
        if (n <= 0) return BadRequest("The number of stories must be greater than 0.");

        var cacheKey = $"BestStories_{n}";
        if (_cache.TryGetValue(cacheKey, out object cachedStories))
        {
            return Ok(cachedStories);
        }

        try
        {
            string storiesUrl = _settings.StoriesUrl;
            string itemUrl = _settings.ItemUrl;
            using var client = _httpClientFactory.CreateClient();
            var response = await client.GetStringAsync(storiesUrl);
            var storyIds = JsonSerializer.Deserialize<int[]>(response);
            if (storyIds == null || storyIds.Length<=0) return StatusCode(500, "Failed to fetch the story IDs due to an error. Please verify the source or try again later.");

            var tasks = storyIds.Take(n).Select(async id =>
            {
                var storyResponse = await client.GetStringAsync(string.Format(itemUrl, id));
                return JsonSerializer.Deserialize<Story>(storyResponse);
            });

            var stories = await Task.WhenAll(tasks);

            // Cache the result for 5 minutes
            _cache.Set(cacheKey, stories, TimeSpan.FromMinutes(5));

            return Ok(stories.OrderByDescending(s => s.Score));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error fetching stories: {ex.Message}");
        }
    }
}
