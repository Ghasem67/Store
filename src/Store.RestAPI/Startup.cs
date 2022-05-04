using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Store.Infrastracture.Application;
using Store.Persistence.EF;
using Store.Persistence.EF.Categories;
using Store.Services.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.RestAPI
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Store.RestAPI", Version = "v1" });
            });
        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<EFDataContext>()
                .WithParameter("connectionString", Configuration["ConnectionString"])
                 .AsSelf()
                 .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(EFCategoryRepository).Assembly)
                      .AssignableTo<Repository>()
                      .AsImplementedInterfaces()
                      .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(CategoryAppService).Assembly)
                      .AssignableTo<Service>()
                      .AsImplementedInterfaces()
                      .InstancePerLifetimeScope();

            builder.RegisterType<EFUnitOfWork>()
                .As<UnitOfWork>()
                .InstancePerLifetimeScope();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Store.RestAPI v1"));
            }
            var environment = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
            var jsonOptions = app.ApplicationServices.GetService<IOptions<JsonOptions>>()?.Value.JsonSerializerOptions;

            app.UseExceptionHandler(_ => _.Run(async context =>
            {
                var exception = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;
                var errorType = exception?.GetType().Name.Replace("Exception", String.Empty);
                var errorDescription = environment.IsProduction() ? null : exception?.ToString();
                var result = new
                {
                    Error = errorType,
                    Description = errorDescription
                };

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = MediaTypeNames.Application.Json;
                await context.Response.WriteAsync(JsonSerializer.Serialize(result, jsonOptions));
            }));
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
