using Microsoft.AspNetCore.Mvc;
using ExternalApiCache.Models;
using ExternalApiCache.Repositories;
using System.Net.Http.Json;

namespace ExternalApiCache.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly PostRepository _repository;
    private readonly IHttpClientFactory _httpClientFactory;

    public PostsController(
        PostRepository repository,
        IHttpClientFactory httpClientFactory)
    {
        _repository = repository;
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var posts = await _repository.GetAllPostsAsync();
            return Ok(posts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Failed to retrieve posts from database",
                error = ex.Message
            });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            // 1.First Check database-cache memory
            var post = await _repository.GetPostByIdAsync(id);

            if (post != null)
                return Ok(post);

            // 2. Call exteral API
            var client = _httpClientFactory.CreateClient();

            HttpResponseMessage response;

            try
            {
                response = await client.GetAsync(
                    $"https://jsonplaceholder.typicode.com/posts/{id}");
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(503, new
                {
                    message = "External API is not reachable",
                    error = ex.Message
                });
            }

            if (!response.IsSuccessStatusCode)
            {
                return NotFound(new
                {
                    message = $"Post with ID {id} not found in external API"
                });
            }

            post = await response.Content.ReadFromJsonAsync<Post>();

            if (post == null)
            {
                return StatusCode(500, new
                {
                    message = "Failed to parse external API response"
                });
            }

            // 3. Save to database
            try
            {
                await _repository.InsertPostAsync(post);
            }
            catch (Exception ex)
            {
                // If DB insert fails, still return API data
                return StatusCode(500, new
                {
                    message = "Post fetched but failed to save in database",
                    error = ex.Message
                });
            }

            return Ok(post);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Unexpected server error",
                error = ex.Message
            });
        }
    }
}