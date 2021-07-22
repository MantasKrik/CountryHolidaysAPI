using CountryHolidaysAPI.Models;
using CountryHolidaysAPI.Repositories;
using CountryHolidaysAPI.Services;
using CountryHolidaysAPI.Services.RepositoryServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CountryHolidaysAPI
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
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            if (bool.Parse(configuration[string.Concat(DatabaseSyncServiceOptions.DatabaseSyncService, ":", "RegisterService")]))
            {
                services.AddHostedService<DatabaseSyncService>();                
                services.AddDbContextFactory<CountryHolidaysContext>(options =>
                {
                    options.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);
                });
                services.Configure<DatabaseSyncServiceOptions>(Configuration.GetSection(DatabaseSyncServiceOptions.DatabaseSyncService));
            }

            services.AddHttpClient();
            services.AddScoped<ICountryRepositoryExtension, CountryRepository>();
            services.AddScoped<IRepository<Region>, RegionRepository>();
            services.AddScoped<IHolidayRepositoryExtension, HolidayRepository>();
            services.AddScoped<IRepository<HolidayName>, HolidayNameRepository>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IHolidayService, HolidayService>();
            services.AddDbContext<CountryHolidaysContext>(o => o.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers();
            
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Country holidays API V1");
            });
        }
    }
}
