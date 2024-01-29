//using CardapioDigital.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Linq;
//using System.Threading.Tasks;

//namespace CardapioDigital.Controller
//{

//    [Route("api/[controller]")]
//    [ApiController]
//    public class IngredientsController : ControllerBase
//    {
//        private readonly IngredientDbContext _context;
//        public IngredientsController(IngredientDbContext context)
//        {
//            _context = context;
//        }

//        // GET: api/IngredientAvailable/{ingredientName}
//        [HttpGet("{ingredientName}")]
//        public async Task<ActionResult<bool>> IsIngredientAvailable(string ingredientName)
//        {
//            var ingredient = await _context.Ingredients.FirstOrDefaultAsync(i => i.Name == ingredientName);

//            if (ingredient == null)
//            {
//                return NotFound();
//            }

//            return ingredient.Available;
//        }


//        // GET: api/Ingredient/{ingredientName}
//        [HttpGet("{ingredientName}")]
//        public async Task<ActionResult<Ingredients>> GetIngredientByTitle(string ingredientName)
//        {
//            var ingredient = await _context.Ingredients.FirstOrDefaultAsync(d => d.Name == ingredientName);

//            if (ingredient == null)
//            {
//                return NotFound();
//            }

//            return ingredient;
//        }

//        // POST: api/Ingredient
//        [HttpPost]
//        public async Task<ActionResult<Ingredients>> AddIngredient([FromBody] Ingredients ingredient)
//        {
//            _context.Ingredients.Add(ingredient);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction(nameof(GetIngredientByTitle), new { title = ingredient.Name }, ingredient);
//        }

//        // PUT: api/Ingredient/{ingredientName}
//        [HttpPut("ingredientName")]
//        public async Task<ActionResult<Ingredients>> UpdateIngredient(string ingredientName, Ingredients ingredient)
//        {
//            if (ingredientName != ingredient.Name)
//            {
//                return BadRequest();
//            }

//            _context.Entry(ingredient).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!IngredientExists(ingredientName))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }
//            return NoContent();
//        }

//        //DELETE: api/Ingredient/{ingredientName}
//        [HttpDelete("ingredientName")]
//        public async Task<ActionResult> DeleteIngredient(string ingredientName)
//        {
//            var ingredient = await _context.Ingredients.FirstOrDefaultAsync(i => i.Name == ingredientName);
//            if (ingredient == null)
//            {
//                return NotFound();
//            }

//            _context.Ingredients.Remove(ingredient);
//            await _context.SaveChangesAsync();
//            return NoContent();
//        }

//        private bool IngredientExists(string ingredientName)
//        {
//            return _context.Ingredients.Any(e => e.Name == ingredientName);
//        }
//    }
//}

