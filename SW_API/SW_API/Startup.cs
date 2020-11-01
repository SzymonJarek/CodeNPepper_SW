using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MediatR;
using System.Reflection;
using Swashbuckle.AspNetCore.Filters;
using System.IO;
using Microsoft.OpenApi.Models;

namespace SW_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMediatR(AppDomain.CurrentDomain.Load("ApplicationLayer"));
            services.AddSwaggerGen(conf =>
            {
                conf.SwaggerDoc("ver1", new Microsoft.OpenApi.Models.OpenApiInfo { 
                    Title = "Star Wars API",
                    Version = "1.0",
                    Description = "Simple Star Wars API",
                    Contact = new OpenApiContact
                    {
                        Name = "Szymon Jarek",
                        Email = "jarek.szymon@gmail.com",
                        Url = new Uri("https://linkedin.com/in/szymonjarek95")
                    }
                });
                conf.EnableAnnotations();
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                conf.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(conf =>
            {
                conf.SwaggerEndpoint("/Swagger/ver1/swagger.json", "Star Wars API");
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
