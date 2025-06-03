using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using StaffingApi.Entities.Bson;
using StaffingApi.Services;

namespace StaffingApi.Controllers;

[Route("v1/[controller]")]
[ApiController]
public class PlayerController(IPlayerService playerService): ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPlayer([FromRoute] string id)
    {
        if (string.IsNullOrEmpty(id) || !ObjectId.TryParse(id, out _))
        {
            return BadRequest("Invalid player ID.");
        }
        return Ok(await playerService.GetAsync(id));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(Player player)
    {
        var createdPlayer = await playerService.CreateAsync(player);
        return CreatedAtAction("GetPlayer", new { id = createdPlayer.Id }, createdPlayer);
    }
}