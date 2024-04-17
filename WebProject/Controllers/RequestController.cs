using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("[controller]")]
public class RequestController : ControllerBase
{
    private static List<Request> requests = new List<Request>();

    [HttpGet]
    public IEnumerable<Request> Get()
    {
        return requests.OrderByDescending(i => i.ResolutionDueDate);
    }

    [HttpPost]
    public IActionResult Post(Request Request)
    {
        requests.Add(Request);
        return CreatedAtAction(nameof(Get), new { id = Request.Id }, Request);
    }

    [HttpPut("{id}")]
    public IActionResult Resolve(int id)
    {
        var Request = requests.FirstOrDefault(i => i.Id == id);
        if (Request == null) return NotFound();
        Request.IsResolved = true;
        return NoContent();
    }
}
