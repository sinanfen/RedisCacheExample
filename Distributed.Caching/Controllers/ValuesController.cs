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
        await _distributedCache.SetStringAsync("name", name);
        await _distributedCache.SetAsync("surname", Encoding.UTF8.GetBytes(surname));
        return Ok();
    }

    [HttpGet("get")]
    public async Task<IActionResult> Get()
    {
        var name = await _distributedCache.GetStringAsync("name");
        var surnameBinary = await _distributedCache.GetAsync("surname");
        var surname = Encoding.UTF8.GetString(surnameBinary);
        return Ok(new
        {
            name,
            surname
        });
    }
}
