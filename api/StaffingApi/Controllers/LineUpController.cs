using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using StaffingApi.Entities.Bson;
using StaffingApi.Entities.Dto;
using StaffingApi.Services;

namespace StaffingApi.Controllers;

[Route("v1/[controller]")]
[ApiController]
public class LineUpController(
    ILineUpService lineUpService,
    IPlayerService playerService
    ): ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLineUp([FromRoute] string id)
    {
        if (string.IsNullOrEmpty(id) || !ObjectId.TryParse(id, out _))
        {
            return BadRequest("Invalid line up ID.");
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
    public async Task<IActionResult> CreateAsync(CreateLineUpDto dto)
    {
        var createdLineUp = await lineUpService.CreateAsync(dto);
        return CreatedAtAction("GetLineUp", new { id = createdLineUp.Id }, createdLineUp);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateAsync(LineUpDto dto)
    {
        if (string.IsNullOrEmpty(dto.Id) || !ObjectId.TryParse(dto.Id, out _))
        {
            return BadRequest("Invalid line up ID.");
        }
        var updatedLineUp = await lineUpService.UpdateAsync(dto);
        if (updatedLineUp == null)
            return NoContent();
        return Ok(updatedLineUp);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] string id)
    {
        if (string.IsNullOrEmpty(id) || !ObjectId.TryParse(id, out _))
        {
            return BadRequest("Invalid line up ID.");
        }
        var deletedCount = await lineUpService.DeleteAsync(id);
        if (deletedCount == 0)
            return NoContent();
        return Ok(deletedCount);
    }

    [HttpPut("add-player")]
    public async Task<IActionResult> AddPlayerAsync([FromBody] AddPlayerToLineUpDto dto)
    {
        if (string.IsNullOrEmpty(dto.LineUpId) || !ObjectId.TryParse(dto.LineUpId, out _))
        {
            return BadRequest("Invalid line up ID.");
        }
        if (string.IsNullOrEmpty(dto.PlayerId) || !ObjectId.TryParse(dto.PlayerId, out _))
        {
            return BadRequest("Invalid player ID.");
        }
        var existingLineUp = await lineUpService.GetAsync(dto.LineUpId);
        if (existingLineUp == null)
        {
            return NotFound($"Line up {dto.LineUpId} not found.");
        }
        var existingPlayer = await playerService.GetAsync(dto.PlayerId);
        if (existingPlayer == null)
        {
            return NotFound($"Player {dto.PlayerId} not found.");
        }
        if (existingLineUp.PlayerIds.Any(playerId => playerId == dto.PlayerId))
        {
            return BadRequest($"Player '{existingPlayer.Name}'({dto.PlayerId}) is already in line up '{existingLineUp.Name}'({dto.LineUpId}).");
        }
        existingLineUp = await lineUpService.AddPlayerAsync(existingLineUp, existingPlayer);
        return Ok(existingLineUp);
    }
    
    [HttpPut("remove-player")]
    public async Task<IActionResult> RemovePlayerAsync([FromBody] AddPlayerToLineUpDto dto)
    {
        if (string.IsNullOrEmpty(dto.LineUpId) || !ObjectId.TryParse(dto.LineUpId, out _))
        {
            return BadRequest("Invalid line up ID.");
        }
        if (string.IsNullOrEmpty(dto.PlayerId) || !ObjectId.TryParse(dto.PlayerId, out _))
        {
            return BadRequest("Invalid player ID.");
        }
        var existingLineUp = await lineUpService.GetAsync(dto.LineUpId);
        if (existingLineUp == null)
        {
            return NotFound($"Line up {dto.LineUpId} not found.");
        }
        var existingPlayer = await playerService.GetAsync(dto.PlayerId);
        if (existingPlayer == null)
        {
            return NotFound($"Player {dto.PlayerId} not found.");
        }
        existingLineUp = await lineUpService.RemovePlayerAsync(existingLineUp, existingPlayer);
        return Ok(existingLineUp);
    }
}