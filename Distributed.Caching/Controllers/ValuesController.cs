using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Runtime.InteropServices;
using System.Text;

namespace Distributed.Caching.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    private readonly IDistributedCache _distributedCache;

    public ValuesController(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }


    [HttpGet("set")]
    public async Task<IActionResult> Set(string name, string surname)
    {
        try
        {
            // Check and remove existing keys to avoid WRONGTYPE error
            if (await _distributedCache.GetAsync("name") != null)
                await _distributedCache.RemoveAsync("name");


            if (await _distributedCache.GetAsync("surname") != null)
                await _distributedCache.RemoveAsync("surname");

            // Set new values
            await _distributedCache.SetStringAsync("name", name, options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(5)
            });
            await _distributedCache.SetAsync("surname", Encoding.UTF8.GetBytes(surname), options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(5)
            });

            return Ok();
        }
        catch (Exception ex)
        {
            // Log the exception (for debugging purposes)
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("get")]
    public async Task<IActionResult> Get()
    {
        try
        {
            var name = await _distributedCache.GetStringAsync("name");
            var surname = await _distributedCache.GetStringAsync("surname");

            return Ok(new
            {
                name,
                surname
            });
        }
        catch (Exception ex)
        {
            // Log the exception (for debugging purposes)
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
}
