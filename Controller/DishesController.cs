using CardapioDigital.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CardapioDigital.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly DishDbContext _context;

        public DishController(DishDbContext context)
        {
            _context = context;
        }

        // GET: api/Dish/{title}
        [HttpGet("{title}")]
        public async Task<ActionResult<Dish>> GetDishByTitle(string title)
        {
            var dish = await _context.Dishes.FirstOrDefaultAsync(d => d.Title == title);

            if (dish == null)
            {
                return NotFound();
            }

            return dish;
        }

        // POST: api/Dish
        [HttpPost]
        public async Task<ActionResult<Dish>> AddDish([FromBody] Dish dish)
        {
            _context.Dishes.Add(dish);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDishByTitle), new { title = dish.Title }, dish);
        }

        // PUT: api/Dish/{title}
        [HttpPut("{title}")]
        public async Task<ActionResult> UpdateDish(string title, Dish dish)
        {
            if (title != dish.Title)
            {
                return BadRequest();
            }

            _context.Entry(dish).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DishExists(title))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Dish/{title}
        [HttpDelete("{title}")]
        public async Task<ActionResult> DeleteDish(string title)
        {
            var dish = await _context.Dishes.FirstOrDefaultAsync(d => d.Title == title);
            if (dish == null)
            {
                return NotFound();
            }

            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DishExists(string title)
        {
            return _context.Dishes.Any(e => e.Title == title);
        }
    }
}