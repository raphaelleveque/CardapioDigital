using System;
using Microsoft.EntityFrameworkCore;

namespace CardapioDigital.Models
{
    public class DishDbContext : DbContext
    {
        public DishDbContext(DbContextOptions<DishDbContext> options) : base(options) { }

        public DbSet<Dish> Dishes { get; set; }
    }
}

