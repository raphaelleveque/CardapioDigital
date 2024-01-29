using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace CardapioDigital.Models
{
    public class Dish
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Ingredient1 { get; set; }
        public string? Ingredient2 { get; set; }
        public string? Ingredient3 { get; set; }
        public string? Ingredient4 { get; set; }
        public string? Ingredient5 { get; set; }
        public string? Ingredient6 { get; set; }
        public string? Ingredient7 { get; set; }
        public string? Ingredient8 { get; set; }
        public string? Ingredient9 { get; set; }
        public string? Ingredient10 { get; set; }

        public Dish()
        {
        }
    }
}

