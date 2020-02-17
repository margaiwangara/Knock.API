using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Knock.API.DbContexts;
using Microsoft.EntityFrameworkCore;
using Knock.API.Services;
using  AutoMapper;

namespace Knock.API
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
            services.AddControllers(action => {
                action.ReturnHttpNotAcceptable = true;
            })
            .AddXmlDataContractSerializerFormatters();
            
            services.AddScoped<IKnockRepository, KnockRepository>();
            
            services.AddAutoMapper(typeof(Startup));
            
            services.AddDbContext<KnockContext>(options => {
                options.UseSqlite(Configuration.GetConnectionString("KnockContext"));
            });
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
        }
    }
}
