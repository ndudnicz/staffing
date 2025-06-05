using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using StaffingApi.Entities.Bson;
using StaffingApi.Services;

namespace StaffingApi.Controllers;

[Route("v1/[controller]")]
[ApiController]
public class PlayerController(
    IPlayerService playerService,
    ILineUpService lineUpService
    ): ControllerBase
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
        if (player.LineUpIds.Any())
        {        
            var lineUps = await lineUpService.GetBulkAsync(player.LineUpIds);
            var unknownLineUps = player.LineUpIds
                .Where(id => lineUps.All(l => l.Id != id))
                .Distinct()
                .ToList();
            if (unknownLineUps.Any())
            {
                return BadRequest($"Invalid lineUp IDs : {string.Join(',', unknownLineUps)}.");
            }
        }
        var createdPlayer = await playerService.CreateAsync(player);
        return CreatedAtAction("GetPlayer", new { id = createdPlayer.Id }, createdPlayer);
    }
}