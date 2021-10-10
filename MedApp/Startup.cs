using MedApp.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using System.Linq;
using MedApp.Data;

namespace MedApp
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
            services.AddLocalization();

            services.AddControllers();
            services.AddScoped<IMedicationRepository, MedicationRepository>();

            services.AddDbContext<MedicationContext>(opt =>
                {
                    opt.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection"),
                      sqlServerOptionsAction: sqlOptions =>
                      {
                          sqlOptions.EnableRetryOnFailure();
                      });
                });


            services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("pt-PT"),
                        new CultureInfo("en-US")
                    };

                    options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                    options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
                    {
                        var languages = context.Request.Headers["Accept-Language"].ToString();
                        var currentLanguage = languages.Split(',').FirstOrDefault();
                        var defaultLanguage = string.IsNullOrEmpty(currentLanguage) ? "en-US" : currentLanguage;

                        if (defaultLanguage != "pt-PT" && defaultLanguage != "en-US")
                        {
                            defaultLanguage = "en-US";
                        }

                        return Task.FromResult(new ProviderCultureResult(defaultLanguage, defaultLanguage));
                    }
                    ));
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var localizeOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizeOptions.Value);

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
