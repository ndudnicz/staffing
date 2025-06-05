using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using StaffingApi.Entities.Bson;
using StaffingApi.Services;

namespace StaffingApi.Controllers;

[Route("v1/[controller]")]
[ApiController]
public class LineUpController(ILineUpService lineUpService): ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLineUp([FromRoute] string id)
    {
        if (string.IsNullOrEmpty(id) || !ObjectId.TryParse(id, out _))
        {
            return BadRequest("Invalid player ID.");
        }
        var result = await lineUpService.GetAsync(id);
        if (result == null)
            return NoContent();
        return Ok(result);
    }
    
    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetLineUpByName([FromRoute] string name)
    {
        var result = await lineUpService.GetByNameAsync(name);
        if (result == null)
            return NoContent();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(LineUp player)
    {
        var createdLineUp = await lineUpService.CreateAsync(player);
        return CreatedAtAction("GetLineUp", new { id = createdLineUp.Id }, createdLineUp);
    }
}