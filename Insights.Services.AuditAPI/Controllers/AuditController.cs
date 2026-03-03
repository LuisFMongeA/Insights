using Insights.Domain.Models;
using Insights.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Insights.AuditAPI.Controllers;

[ApiController]
[Route("api/audit")]
public class AuditController(IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuditEntry>>> GetAll()
    {
        var entries = await unitOfWork.AuditRepository.GetAllAsync();
        return Ok(entries);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AuditEntry>> GetById(Guid id)
    {
        var entry = await unitOfWork.AuditRepository.GetByIdAsync(id);

        if (entry is null)
            return NotFound($"Audit entry {id} not found");

        return Ok(entry);
    }

    [HttpGet("range")]
    public async Task<ActionResult<IEnumerable<AuditEntry>>> GetByDateRange(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to)
    {
        if (from > to)
            return BadRequest("'from' date must be earlier than 'to' date");

        var entries = await unitOfWork.AuditRepository.GetByDateRangeAsync(from, to);
        return Ok(entries);
    }
}