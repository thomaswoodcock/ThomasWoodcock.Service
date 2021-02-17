using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using ThomasWoodcock.Service.WebApi.Controllers;

namespace ThomasWoodcock.Service
{
    public sealed class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddApplicationPart(typeof(AccountController).Assembly);
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}