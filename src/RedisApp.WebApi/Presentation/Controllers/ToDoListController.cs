using Microsoft.AspNetCore.Mvc;
using RedisApp.WebApi.Application.Interfaces;
using RedisApp.WebApi.Application.ViewModels;
using RedisApp.WebApi.Infrastructure.Caching;

namespace RedisApp.WebApi.Presentation.Controllers;

[ApiController]
[Route("todos")]
public sealed class ToDoListController : ControllerBase
{
    private readonly IToDoServices _todoServices;

    public ToDoListController(IToDoServices todoServices)
    {
        _todoServices = todoServices;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _todoServices.GetAll());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            return Ok(await _todoServices.GetById(id));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ToDoRequestViewModel viewModel)
    {
        try
        {
            await _todoServices.Create(viewModel);

            return Ok("ToDo created successfully!");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ToDoRequestViewModel viewModel)
    {
        try
        {
            await _todoServices.Update(id, viewModel);

            return Ok("ToDo updated successfully!");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("activate/{id}")]
    public async Task<IActionResult> Activate(Guid id)
    {
        try
        {
            await _todoServices.Activate(id);

            return Ok("ToDo activated successfully!");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("deactivate/{id}")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        try
        {
            await _todoServices.Deactivate(id);

            return Ok("ToDo deactivated successfully!");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}