using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Repositories.Interfaces;
using WebApplication1.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using WebApplication1.Helpers;
using WebApplication1.Extensions;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        readonly string AllowSpecificOrigins = "_AllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            //DependencyInjection
            Helpers.AppSettings appSettings = ConfigureDependencyInjection(services);
            System.Reflection.Assembly assembly = System.Reflection.Assembly.Load(appSettings.DependencyInjection.ControllerAssembly);

            services.AddMvc(options =>
            {
                //options.Filters.AddService<Helpers.UserLoggedFilter>();
                options.EnableEndpointRouting = false;
            })//.SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                            .AddApplicationPart(assembly)
                            .AddControllersAsServices();

            // Cache
            services.AddDistributedMemoryCache();
            services.AddMemoryCache();

            services.AddCors(options =>
            {
                options.AddPolicy(name: AllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("https://localhost:44373")
                                      .AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader();
                                  });
            });

            
            // Register the Swagger services
            //services.AddSwaggerDocument();
            /*services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApplication1", Version = "v1" });
                //c.OperationFilter<Filter.AuthorizationHeaderParameterOperationFilter>();
            });*/
            services
                //.AddCustomMvc()
               .AddSwagger()
               //.AddJwtAuthenticationServices(Configuration, appSettings)
               .AddHttpContextAccessor()
               //.AddSingleton(Log.Logger)
               ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /*if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }*/

            //Helpers.AppSettings appSettings = ConfigureDependencyInjection(services);

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            //app.AddJwtAuthenticationServices(Configuration, appSettings);
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger v1"));

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // Definindo a cultura padrão: pt-BR
            var supportedCultures = new[] { new CultureInfo("pt-BR") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture: "pt-BR", uiCulture: "pt-BR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            //cors
            app.UseCors(AllowSpecificOrigins);
        }

        private Helpers.AppSettings LoadConfigurationFromAppSettings()
        {
            Helpers.AppSettings appSettings = new Helpers.AppSettings();
            new Microsoft.Extensions.Options.ConfigureFromConfigurationOptions<Helpers.AppSettings>(Configuration).Configure(appSettings);
            return appSettings;
        }

        private Helpers.AppSettings ConfigureDependencyInjection(IServiceCollection services)
        {
            Helpers.AppSettings appSettings = LoadConfigurationFromAppSettings();
            services.AddSingleton(appSettings);
            services.AddSingleton(appSettings.DatabaseConfiguration);
            AddLayerDependencyInjection(services, typeof(IContext), appSettings.DependencyInjection.RepositoryAssembly);
            AddLayerDependencyInjection(services, typeof(IRepository), appSettings.DependencyInjection.RepositoryAssembly);
            AddLayerDependencyInjection(services, typeof(IService), appSettings.DependencyInjection.ServiceAssembly);
            return appSettings;
        }



        private void AddLayerDependencyInjection(IServiceCollection services, Type referenceType, string assemblyString)
        {
            var assembly = System.Reflection.Assembly.Load(assemblyString);
            var types = from type in assembly.GetTypes()
                        where referenceType.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract
                        select type;
            foreach (Type type in types)
            {
                services.AddScoped(type, type);
            }
        }
    }
}
