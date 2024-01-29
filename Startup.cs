using CardapioDigital.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CardapioDigital
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "CardapioDigital API", Version = "v1" });
            });

            services.AddDbContext<DishDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("DishesConnection"),
                                 new MySqlServerVersion(new Version(8, 2, 0))));

            services.AddDbContext<IngredientDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("IngredientsConnection"),
                                 new MySqlServerVersion(new Version(8, 2, 0))));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CardapioDigital API v1");
                    c.RoutePrefix = string.Empty; // Define a raiz para a página do Swagger
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // Adiciona suporte a controllers
            });
        }
    }
}
