using Insights.AuditAPI.Data;
using Insights.Services.AuditAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace Insights.AuditAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuditController(IAuditRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuditEntry>>> GetAll()
    {
        var entries = await repository.GetAllAsync();
        return Ok(entries);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AuditEntry>> GetById(Guid id)
    {
        var entry = await repository.GetByIdAsync(id);

        if (entry is null)
            return NotFound($"Audit entry {id} not found");

        return Ok(entry);
    }
}