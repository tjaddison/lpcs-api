using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ResidentSupervision.WebUI {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddMediatR (typeof (Startup).Assembly);

            services.AddCors (options => {
                options.AddPolicy ("CorsPolicy",
                    builder => builder.AllowAnyOrigin ()
                    .AllowAnyMethod ()
                    .AllowAnyHeader ()
                    .AllowCredentials ());
            });

            // Register database
            //services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:DbConnection"]));

            // Config Azure AD B2C stuff
            services.AddAuthentication (options => {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer (jwtOptions => {
                    jwtOptions.Authority = $"https://login.microsoftonline.com/tfp/{Configuration["AzureAdB2C:Tenant"]}/{Configuration["AzureAdB2C:Policy"]}/v2.0/";
                    jwtOptions.Audience = Configuration["AzureAdB2C:ClientId"];

                    // NOTE: Do not use this handler in Production.
                    // jwtOptions.Events = new JwtBearerEvents
                    // {
                    //     OnAuthenticationFailed = AuthenticationFailed
                    // };
                });

            // Config object mappings
            var config = new AutoMapper.MapperConfiguration (cfg => {
                cfg.AddProfile (new AutoMapperProfileConfiguration ());
            });

            var mapper = config.CreateMapper ();
            services.AddSingleton (mapper);

            services.AddMvc ().SetCompatibilityVersion (CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
            app.UseAuthentication ();

            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            } else {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts ();
            }

            app.UseHttpsRedirection ();
            app.UseCors ("CorsPolicy");
            app.UseMvc ();
        }
    }
}