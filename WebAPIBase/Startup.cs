using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dominio.Repositorios;
using API.Dominio.Servicios;
using API.Persistencia.Contextos;
using API.Persistencia.Repositorios;
using API.Servicios;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AutoMapper;

namespace WebAPI
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
            //CORS 
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                            .WithOrigins("*")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            });

            //Base de .net core
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //Se agrega contexto para administrar la base de datos con EF Core
            services.AddDbContext<AppDbContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("StaffingConnectionString"));
            });

            // Cache en memoria
            services.AddMemoryCache();
            // respuesta de almacenamiento en caché para middlewares
            services.AddResponseCaching();

            //Unidad de trabajo: unificación de las tareas asíncronas.
            services.AddScoped<IUnidadDeTrabajo, UnidadDeTrabajo>();

            //Servicios del Web API
            //services.AddTransient<IServicioBase<TipoSimulacion>, ServicioBase<TipoSimulacion>>();

            //Útil para mapear las clases
            services.AddAutoMapper(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //CORS
            app.UseCors("AllowAll");

            // caching response for middlewares
            app.UseResponseCaching();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
