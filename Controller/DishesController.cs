﻿using CardapioDigital.Models;
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
        private readonly IngredientsController _ingredientsController;

        public DishController(DishDbContext context, IngredientsController ingredientsController)
        {
            _context = context;
            _ingredientsController = ingredientsController;
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

        // POST: api/Dish/AddDish
        [HttpPost("AddDish")]
        public async Task<ActionResult<Dish>> AddDish([FromBody] Dish dish)
        {
            await EnsureIngredientsExist(dish);
            _context.Dishes.Add(dish);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDishByTitle), new { title = dish.Title }, dish);
        }

        // PUT: api/Dish/{title}
        [HttpPut("{title}")]
        public async Task<ActionResult> UpdateDish(string title, Dish updatedDish)
        {
            var existingDish = await _context.Dishes.FirstOrDefaultAsync(d => d.Title == title);

            if (existingDish == null)
            {
                return NotFound();
            }

            // Copiar as propriedades modificáveis do updatedDish para o existingDish
            foreach (var property in typeof(Dish).GetProperties())
            {
                // Ignorar a propriedade Id (chave primária)
                if (property.Name != "Id")
                {
                    // Obter o valor da propriedade no updatedDish
                    var updatedValue = property.GetValue(updatedDish);

                    // Atribuir o valor à propriedade correspondente no existingDish
                    property.SetValue(existingDish, updatedValue);
                }
            }

            // Marcar a chave primária como não modificada
            _context.Entry(existingDish).Property(x => x.Id).IsModified = false;

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

        private async Task EnsureIngredientsExist(Dish dish)
        {
            var allIngredientsResult = await _ingredientsController.GetAllIngredients();

            if (allIngredientsResult.Result is OkObjectResult okResult && okResult.Value is HashSet<string> allIngredients)
            {
                for (int i = 1; i <= 10; i++)
                {
                    var ingredientPropertyName = $"Ingredient{i}";
                    var ingredientValue = dish.GetType().GetProperty(ingredientPropertyName)?.GetValue(dish) as string;

                    if (ingredientValue == null)
                    {
                        break;
                    }

                    if (!allIngredients.Contains(ingredientValue))
                    {
                        await _ingredientsController.AddIngredient(new Ingredients { Name = ingredientValue, Available = false });
                        allIngredients.Add(ingredientValue);
                    }
                }
            }
        }
    }
}