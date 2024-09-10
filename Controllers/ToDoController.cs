using System.Collections.Generic;
using TodoApi.Models;
using TodoApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ToDoApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ToDoController : ControllerBase
  {
    private readonly ToDoService _todoService;

    public ToDoController(ToDoService todoService)
    {
      _todoService = todoService;
    }
    [HttpGet]
    public async Task<ActionResult<List<ToDoItem>>> Get()
    {
      var items = await _todoService.GetAsync();
      return Ok(items);
    }
    [HttpGet("{id:length(24)}", Name = "GetToDo")]
    public async Task<ActionResult<ToDoItem>> Get(string id)
    {
      var item = await _todoService.GetAsync(id);
      if (item == null)
      {
        return NotFound();
      }
      return Ok(item);
    }
    [HttpPost]
    public async Task<ActionResult<ToDoItem>> Create(ToDoItem newToDo)
    {
      await _todoService.CreateAsync(newToDo);
      return CreatedAtRoute("GetToDo", new { id = newToDo.Id }, newToDo);
    }
    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, ToDoItem updateToDo)
    {
      var item = await _todoService.GetAsync(id);
      if (item == null)
      {
        return NotFound();
      }
      await _todoService.UpdateAsync(id, updateToDo);
      return NoContent();
    }
    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
      var item = await _todoService.GetAsync(id);
      if (item == null)
      {
        return NotFound();
      }
      await _todoService.RemoveAsync(item);
      return NoContent();
    }

  }

}