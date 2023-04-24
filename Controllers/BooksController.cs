using StatsApi.Models;
using StatsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace StatsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FavoriteController : ControllerBase
{
    private readonly StatsService _statsService;

    public FavoriteController(StatsService statsService) =>
        _statsService = statsService;

    [HttpGet]
    public async Task<List<Favorite>> Get() =>
        await _statsService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Favorite>> Get(string id)
    {
        var book = await _statsService.GetAsync(id);

        if (book is null)
        {
            return NotFound();
        }

        return book;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Favorite newBook)
    {
        await _statsService.CreateAsync(newBook);

        return CreatedAtAction(nameof(Get), new { id = newBook.Id }, newBook);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Favorite updatedBook)
    {
        var book = await _statsService.GetAsync(id);

        if (book is null)
        {
            return NotFound();
        }

        updatedBook.Id = book.Id;

        await _statsService.UpdateAsync(id, updatedBook);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var book = await _statsService.GetAsync(id);

        if (book is null)
        {
            return NotFound();
        }

        await _statsService.RemoveAsync(id);

        return NoContent();
    }
}