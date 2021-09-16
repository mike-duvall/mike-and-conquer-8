using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace mike_and_conquer_simulation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Console.WriteLine("Enter Startup(IConfiguration configuration)");
            Configuration = configuration;
            var configInfo = (Configuration as IConfigurationRoot).GetDebugView();
            Console.WriteLine("****************");
            Console.WriteLine("configInfo=" + configInfo);
            Console.WriteLine("****************");
            Console.WriteLine("Exit Startup(IConfiguration configuration)");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine("Entering Startup::ConfigureServices(IServiceCollection services)");
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "testrest", Version = "v1" });
            });

            Console.WriteLine("Leaving Startup::ConfigureServices(IServiceCollection services)");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Console.WriteLine("Entering Startup::Configure(IApplicationBuilder app, IWebHostEnvironment env)");
            if (env.IsDevelopment())
            {
                Console.WriteLine("Startup::Configure(IApplicationBuilder app, IWebHostEnvironment env)-IsDevelopment = true");
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "testrest v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            Console.WriteLine("Leaving Startup::Configure(IApplicationBuilder app, IWebHostEnvironment env)");
        }
    }
}
