using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemory.Caching.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    private readonly IMemoryCache _memoryCache;

    public ValuesController(IMemoryCache memoryCache)
    {
        this._memoryCache = memoryCache;
    }

    [HttpGet("set/{name}")]
    public void SetName(string name)
    {
        _memoryCache.Set("name", name);
    }

    [HttpGet]
    public string GetName()
    {
        if (_memoryCache.TryGetValue<string>("name", out string name))
            return name;
        return "";
    }

    [HttpGet("setDate")]
    public void SetDate()
    {
        _memoryCache.Set<DateTime>("date", DateTime.UtcNow, options: new()
        {
            AbsoluteExpiration = DateTime.Now.AddSeconds(30),
            SlidingExpiration = TimeSpan.FromSeconds(5),
        });
    }

    [HttpGet("getDate")]
    public DateTime GetDate()
    {
        return _memoryCache.Get<DateTime>("date");
    }
}
