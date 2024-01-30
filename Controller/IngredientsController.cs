using CardapioDigital.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CardapioDigital.Controller
{

    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        private readonly IngredientDbContext _context;
        public IngredientsController(IngredientDbContext context)
        {
            _context = context;
        }

        // GET: api/IsIngredientAvailable/{ingredientName}
        [HttpGet("IsIngredientAvailable/{ingredientName}")]
        public async Task<ActionResult<bool>> IsIngredientAvailable(string ingredientName)
        {
            var ingredient = await _context.Ingredients.FirstOrDefaultAsync(i => i.Name == ingredientName);

            if (ingredient == null)
            {
                return NotFound();
            }

            return ingredient.Available;
        }


        // GET: api/Ingredient/{ingredientName}
        [HttpGet("{ingredientName}")]
        public async Task<ActionResult<Ingredients>> GetIngredientByTitle(string ingredientName)
        {
            var ingredient = await _context.Ingredients.FirstOrDefaultAsync(d => d.Name == ingredientName);

            if (ingredient == null)
            {
                return NotFound();
            }

            return ingredient;
        }

        // GET: api/GetAllIngredients/
        [HttpGet("GetAllIngredients")]
        public async Task<ActionResult<HashSet<string>>> GetAllIngredients()
        {
            var ingredients = await _context.Ingredients.Select(i => i.Name).ToListAsync();
            var uniqueIngredients = new HashSet<string>(ingredients);

            return Ok(uniqueIngredients);
        }

        // GET: api/GetAllIngredientsAndAvailability/
        [HttpGet("GetAllIngredientsAndAvailability")]
        public async Task<ActionResult<Dictionary<string, bool>>> GetAllIngredientsAndAvailability()
        {
            var ingredients = await _context.Ingredients.ToListAsync();
            var uniqueIngredients = ingredients.ToDictionary(i => i.Name, i => i.Available);

            return Ok(uniqueIngredients);
        }

        // POST: api/Ingredient
        [HttpPost("Ingredient")]
        public async Task<ActionResult<Ingredients>> AddIngredient([FromBody] Ingredients ingredient)
        {
            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetIngredientByTitle), new { ingredientName = ingredient.Name }, ingredient);
        }

        // PUT: api/UpdateIngredientAvailability/{ingredientName}/{available}
        [HttpPut("UpdateIngredientAvailability/{ingredientName}/{available}")]
        public async Task<ActionResult<Ingredients>> UpdateIngredientAvailability(string ingredientName, bool available)
        {
            var existingIngredient = await _context.Ingredients.FirstOrDefaultAsync(d => d.Name == ingredientName);

            if (existingIngredient == null)
            {
                return NotFound();
            }

            existingIngredient.Available = available;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngredientExists(ingredientName))
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

        // PUT: api/UpdateIngredientName/{oldName}/{newName}
        [HttpPut("UpdateIngredientName/{oldName}/{newName}")]
        public async Task<ActionResult<Ingredients>> UpdateIngredientName(string oldName, string newName)
        {
            var existingIngredient = await _context.Ingredients.FirstOrDefaultAsync(d => d.Name == oldName);

            if (existingIngredient == null)
            {
                return NotFound();
            }

            existingIngredient.Name = newName;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngredientExists(newName))
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

        //DELETE: api/Ingredient/{ingredientName}
        [HttpDelete("ingredientName")]
        public async Task<ActionResult> DeleteIngredient(string ingredientName)
        {
            var ingredient = await _context.Ingredients.FirstOrDefaultAsync(i => i.Name == ingredientName);
            if (ingredient == null)
            {
                return NotFound();
            }

            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("LetAllIngredientsAvailable")]
        public async Task<ActionResult> LetAllIngredientsAvailable()
        {
            await _context.Ingredients
                .ForEachAsync(ingredient => ingredient.Available = true);

            await _context.SaveChangesAsync();

            return NoContent();
        }



        private bool IngredientExists(string ingredientName)
        {
            return _context.Ingredients.Any(e => e.Name == ingredientName);
        }
    }
}

