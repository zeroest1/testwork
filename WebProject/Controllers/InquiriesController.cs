using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("[controller]")]
public class InquiriesController : ControllerBase
{
    private static List<Inquiry> _inquiries = new List<Inquiry>();

    [HttpGet]
    public IEnumerable<Inquiry> Get()
    {
        return _inquiries.OrderByDescending(i => i.ResolutionDueDate);
    }

    [HttpPost]
    public IActionResult Post(Inquiry inquiry)
    {
        _inquiries.Add(inquiry);
        return CreatedAtAction(nameof(Get), new { id = inquiry.Id }, inquiry);
    }

    [HttpPut("{id}")]
    public IActionResult Resolve(int id)
    {
        var inquiry = _inquiries.FirstOrDefault(i => i.Id == id);
        if (inquiry == null) return NotFound();
        inquiry.IsResolved = true;
        return NoContent();
    }
}
