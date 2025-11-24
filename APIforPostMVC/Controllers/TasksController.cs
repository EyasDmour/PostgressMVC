using System.Security.Claims;
using APIforPostMVC.Data.Service;
using APIforPostMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace APIforPostMVC.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITasksService _service;

    public TasksController(ITasksService service)
    {
        _service = service;
    }

    // GET: api/Tasks
    [HttpGet]
    public async Task<IEnumerable<Tasks>> Get()
    {
        return await _service.GetAll();
    }

    // POST: api/Tasks
    [HttpPost]
    public async Task<IActionResult> Post(Tasks task)
    {
        await _service.Add(task);
        return CreatedAtAction(nameof(Get), new { id = task.Id }, task);
    }

    // PUT: api/Tasks/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Tasks task)
    {
        if (id != task.Id) return BadRequest();
        
        try
        {
            var userId = GetUserId();
            await _service.Update(task, userId);
            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
    }

    // DELETE: api/Tasks/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var userId = GetUserId();
            await _service.Delete(id, userId);
            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub) ?? User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
        {
            return userId;
        }
        throw new UnauthorizedAccessException("User ID not found in token.");
    }
}
