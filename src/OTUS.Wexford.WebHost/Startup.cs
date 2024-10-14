/*
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OTUS.Wexford.Core.Abstractions.Repositories;
using OTUS.Wexford.Core.Domain.Administration;
using OTUS.Wexford.DataAccess.Data;
using OTUS.Wexford.DataAccess.Repositories;

namespace OTUS.Wexford.WebHost
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton(typeof(IRepository<Employee>), (x) => new InMemoryRepository<Employee>(FakeDataFactory.Employees));
            services.AddSingleton(typeof(IRepository<Role>), (x) => new InMemoryRepository<Role>(FakeDataFactory.Roles));

            services.AddOpenApiDocument(options =>
            {
                options.Title = "PromoCode Factory API Doc";
                options.Version = "1.0";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseOpenApi();

            app.UseSwaggerUi(x =>
            {
                x.DocExpansion = "list";
            });



            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
*/