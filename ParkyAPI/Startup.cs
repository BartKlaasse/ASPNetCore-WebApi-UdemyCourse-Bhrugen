using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ParkyAPI.Data;
using ParkyAPI.ParkyMapper;
using ParkyAPI.Repository;
using ParkyAPI.Repository.IRepository;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ParkyAPI
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
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<INationalParkRepository, NationalParkRepository>();
            services.AddScoped<ITrailRepository, TrailRepository>();
            services.AddAutoMapper(typeof(ParkyMappings));
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();
            // services.AddSwaggerGen(options =>
            // {
            //     // options.SwaggerDoc("ParkyOpenApiSpecNP",
            //     //     new Microsoft.OpenApi.Models.OpenApiInfo()
            //     //     {
            //     //         Title = "ParkyApi NationalParks",
            //     //             Version = "1",
            //     //             Description = "Udemy Parky Api for national parkscourse by bhrugen patel",
            //     //             Contact = new Microsoft.OpenApi.Models.OpenApiContact()
            //     //             {
            //     //                 Email = "name@example.com",
            //     //                     Name = "Bart Klaasse",
            //     //                     Url = new Uri("Https://google.com")
            //     //             },
            //     //             License = new Microsoft.OpenApi.Models.OpenApiLicense()
            //     //             {
            //     //                 Name = "Custom license",
            //     //                     Url = new Uri("https://google.com")
            //     //             }
            //     //     });
            //     // options.SwaggerDoc("ParkyOpenApiSpecTrails",
            //     //     new Microsoft.OpenApi.Models.OpenApiInfo()
            //     //     {
            //     //         Title = "ParkyApi Trails",
            //     //             Version = "1",
            //     //             Description = "Udemy Parky Api for trails course by bhrugen patel",
            //     //             Contact = new Microsoft.OpenApi.Models.OpenApiContact()
            //     //             {
            //     //                 Email = "name@example.com",
            //     //                     Name = "Bart Klaasse",
            //     //                     Url = new Uri("Https://google.com")
            //     //             },
            //     //             License = new Microsoft.OpenApi.Models.OpenApiLicense()
            //     //             {
            //     //                 Name = "Custom license",
            //     //                     Url = new Uri("https://google.com")
            //     //             }
            //     //     });                
            //     options.SwaggerDoc("ParkyOpenApiSpec",
            //         new Microsoft.OpenApi.Models.OpenApiInfo()
            //         {
            //             Title = "ParkyApi",
            //                 Version = "1",
            //                 Description = "Udemy Parky Api for course by bhrugen patel",
            //                 Contact = new Microsoft.OpenApi.Models.OpenApiContact()
            //                 {
            //                     Email = "name@example.com",
            //                         Name = "Bart Klaasse",
            //                         Url = new Uri("Https://google.com")
            //                 },
            //                 License = new Microsoft.OpenApi.Models.OpenApiLicense()
            //                 {
            //                     Name = "Custom license",
            //                         Url = new Uri("https://google.com")
            //                 }
            //         });
            //     //FLOW: Get the xml file from the root folder of the project.
            //     var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //     var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
            //     options.IncludeXmlComments(xmlCommentsFullPath);
            // });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            // app.UseSwaggerUI(options =>
            // {
            //     // options.SwaggerEndpoint("/swagger/ParkyOpenApiSpecNP/swagger.json", "Parky Api National Parks");
            //     // options.SwaggerEndpoint("/swagger/ParkyOpenApiSpecTrails/swagger.json", "Parky Api Trails");
            //     options.SwaggerEndpoint("/swagger/ParkyOpenApiSpec/swagger.json", "Parky Api");
            //     options.RoutePrefix = "";
            // });
            app.UseSwaggerUI(options =>
            {
                foreach (var desc in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.GroupName.ToUpperInvariant());
                }
                options.RoutePrefix = "";
            });
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}