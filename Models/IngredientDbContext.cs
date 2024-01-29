using System;
using Microsoft.EntityFrameworkCore;

namespace CardapioDigital.Models
{
    public class IngredientDbContext : DbContext
    {
        public IngredientDbContext(DbContextOptions<IngredientDbContext> options) : base(options) { }

        public DbSet<Ingredients> Ingredients { get; set; }
    }

}

