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
    public async Task<IActionResult> CreateAsync(CreatePlayerDto dto)
    {
        if (dto.LineUpIds.Length > 0)
        {
            foreach (var lineUpId in dto.LineUpIds)
            {
                if (string.IsNullOrEmpty(lineUpId) || !ObjectId.TryParse(lineUpId, out _))
                {
                    return BadRequest($"Invalid line up ID: {lineUpId}.");
                }
            }
            var lineUps = await lineUpService.GetBulkAsync(dto.LineUpIds);
            var unknownLineUps = dto.LineUpIds
                .Where(id => lineUps.All(l => l.Id != id))
                .Distinct()
                .ToList();
            if (unknownLineUps.Any())
            {
                return BadRequest($"Invalid line up IDs : {string.Join(',', unknownLineUps)}.");
            }
        }
        var createdPlayer = await playerService.CreateAsync(dto);
        return CreatedAtAction("GetPlayer", new { id = createdPlayer.Id }, createdPlayer);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateAsync(PlayerDto dto)
    {
        if (string.IsNullOrEmpty(dto.Id) || !ObjectId.TryParse(dto.Id, out _))
        {
            return BadRequest("Invalid player ID.");
        }
        if (dto.LineUpIds.Length > 0)
        {
            foreach (var lineUpId in dto.LineUpIds)
            {
                if (string.IsNullOrEmpty(lineUpId) || !ObjectId.TryParse(lineUpId, out _))
                {
                    return BadRequest($"Invalid line up ID: {lineUpId}.");
                }
            }
            var lineUps = await lineUpService.GetBulkAsync(dto.LineUpIds);
            var unknownLineUps = dto.LineUpIds
                .Where(id => lineUps.All(l => l.Id != id))
                .Distinct()
                .ToList();
            if (unknownLineUps.Any())
            {
                return BadRequest($"Invalid line up IDs : {string.Join(',', unknownLineUps)}.");
            }
        }
        var updatedPlayer = await playerService.UpdateAsync(dto);
        if (updatedPlayer == null)
            return NoContent();
        return Ok(updatedPlayer);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] string id)
    {
        if (string.IsNullOrEmpty(id) || !ObjectId.TryParse(id, out _))
        {
            return BadRequest("Invalid player ID.");
        }
        var deletedCount = await playerService.DeleteAsync(id);
        if (deletedCount == 0)
            return NoContent();
        return Ok(deletedCount);
    }
}