using System.Net;
using Database.Tables;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Service;

namespace Server.Controllers;
[ApiController]
[Route("api/[controller]")]
public class _BaseController<TModel, TTable> : ControllerBase
where TModel : class
    where TTable : _BaseTable
{
    private readonly _IBaseService<TModel, TTable> _serviceBase;

    public _BaseController(_IBaseService<TModel, TTable> service)
    {
        _serviceBase = service ?? throw new ArgumentNullException(nameof(service));
    }

    // GET: api/[controller]
    [HttpGet]
    public virtual async Task<ActionResult<IEnumerable<TModel>>> GetAll()
    {
        var items = await _serviceBase.GetAllAsync();
        return Ok(items);
    }

    // GET: api/[controller]/{id}
    [HttpGet("{id}")]
    public virtual async Task<ActionResult<TModel>> GetById(ulong id)
    {
        var item = await _serviceBase.GetByIdAsync(id);
        if (item == null)
            return NotFound();

        return Ok(item);
    }

    // POST: api/[controller]
    [HttpPost]
    public virtual async Task<ActionResult<TModel>> Create([FromBody] TModel model)
    {
        if (model == null)
            return BadRequest("Model cannot be null");

        var createdItem = await _serviceBase.CreateAsync(model);
        return CreatedAtAction(nameof(GetById), new { id = ((dynamic)createdItem).Id }, createdItem);
    }

    // PUT: api/[controller]/{id}
    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(int id, [FromBody] TModel model)
    {
        if (model == null)
            return BadRequest("Model cannot be null");

        // Ensure the ID is part of the model
        ((dynamic)model).Id = id;

        await _serviceBase.UpdateAsync(model);
        return NoContent();
    }

    // DELETE: api/[controller]/{id}
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(ulong id)
    {
        await _serviceBase.DeleteAsync(id);
        return NoContent();
    }
}