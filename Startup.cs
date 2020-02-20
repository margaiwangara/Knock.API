using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Knock.API.DbContexts;
using Microsoft.EntityFrameworkCore;
using Knock.API.Services;
using  AutoMapper;
using Knock.API.Helpers;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Knock.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(action => {
                action.ReturnHttpNotAcceptable = true;
            })
            .AddXmlDataContractSerializerFormatters();
            
            services.AddDbContext<KnockContext>(options => {
                options.UseSqlite(Configuration.GetConnectionString("KnockContext"));
            });

            services.AddScoped<IKnockRepository, KnockRepository>();
            services.AddAutoMapper(typeof(Startup));
            
            // auth code
            services.AddCors();
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            
            services.AddAuthentication(a => 
            {
                a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(a => 
            {
                a.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context => 
                    {
                        var knockRepository = context.HttpContext.RequestServices.GetRequiredService<IKnockRepository>();
                        var userId = Guid.Parse(context.Principal.Identity.Name);

                        var user = knockRepository.GetUserAsync(userId);
                
                        if(user == null)
                        {
                            context.Fail("Unauthorized Access");
                        }

                        return Task.CompletedTask;
                    }
                };
                a.RequireHttpsMetadata = false;
                a.SaveToken = true;
                a.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false                   
                };
            });
            // end of auth code
            
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

            // enable cors
            app.UseCors(x => x
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            
            // enable auth
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
