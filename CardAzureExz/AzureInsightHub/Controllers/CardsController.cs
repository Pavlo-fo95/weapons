using AzureInsightHub.Data;
using AzureInsightHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AzureInsightHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public CardsController(DatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получение списка видимых карточек.
        /// </summary>
        /// <param name="category">Категория карточек.</param>
        /// <param name="sortBy">Сортировка по "date" или "category".</param>
        /// <returns>Список видимых карточек.</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetVisibleCards([FromQuery] string? category, [FromQuery] string? sortBy)
        {
            var query = _context.Cards.Where(card => card.Visible);

            if (!string.IsNullOrEmpty(category))
                query = query.Where(card => card.Category == category);

            if (sortBy == "date")
                query = query.OrderBy(card => card.DateAdded);
            else if (sortBy == "category")
                query = query.OrderBy(card => card.Category);

            return Ok(await query.ToListAsync());
        }

        /// <summary>
        /// Получение карточки по ID.
        /// </summary>
        /// <param name="id">ID карточки.</param>
        /// <returns>Карточка с указанным ID.</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCardById(int id)
        {
            var card = await _context.Cards.FindAsync(id);
            if (card == null || !card.Visible)
                return NotFound("Карточка не найдена или недоступна.");

            return Ok(card);
        }

        [HttpPost]
        public async Task<IActionResult> AddCard([FromBody] Card card)
        {
            card.DateAdded = DateTime.UtcNow;
            _context.Cards.Add(card);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCardById), new { id = card.Id }, card);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCard(int id, [FromBody] Card updatedCard)
        {
            var card = await _context.Cards.FindAsync(id);
            if (card == null) return NotFound("Карточка не найдена.");

            card.Name = updatedCard.Name;
            card.Category = updatedCard.Category;
            card.Description = updatedCard.Description;
            card.PhotoUrl = updatedCard.PhotoUrl;
            card.Visible = updatedCard.Visible;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCard(int id)
        {
            var card = await _context.Cards.FindAsync(id);
            if (card == null) return NotFound("Карточка не найдена.");

            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
