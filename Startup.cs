using MkeAlerts.Web.Data;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Text;
using VueCliMiddleware;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data;
using MkeAlerts.Web.Services;
using MkeAlerts.Web.Services.Data;
using FluentValidation.AspNetCore;
using FluentValidation;
using MkeAlerts.Web.Middleware.Exceptions;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Filters;
using Hangfire;
using MkeAlerts.Web.Models.Data.Properties;

namespace MkeAlerts.Web
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
            // Add the database context
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default"), x => x.UseNetTopologySuite()));

            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("Default")));
            services.AddHangfireServer();

            // Add Identity
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // ===== Add Jwt Authentication ========
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration["JwtIssuer"],
                        ValidAudience = Configuration["JwtIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });

            var mapperConfiguration = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new DataModelsProfile());
            });
            IMapper mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);

            // Add MVC
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Formatting = Formatting.Indented;
                    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Add Swagger (via Swashbuckle)
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Milwaukee Alerts API",
                    Version = "v1",
                    Description = @"
## Authentication

Many of these API endpoints are only available to authenticated users. To authenticate, make a call to `GET /api/Account/Login`. Assuming valid credentials are provided, a JWT token will be
returned. This token should be included in the `Authorization` header as a bearer token on subsequent requests.

Within Swagger UI, you can click the 'Authorize' button to use a token on requests. Make sure that the value entered includes the word `Bearer` before the token -- the value Swagger UI is
sending is the entire `Authorization` header.

## Errors

A 4xx or 5xx HTTP status code indicates an error. Errors include an `ErrorDetails` object (or an object derived from `ErrorDetails`, such as `ValidationErrorDetails`) in the response with 
additional details about the error.

## Filtering Results

Results can be filtered with the `filter` parameter. You can use basic conditional operations, parentheses for grouping, null checks, and operators like AND and OR. Here are some examples:
* `state = ""WI""`
* `state = ""WI"" or State = ""CA""`
* `state = null`
* `state != null`
* `state = ""WI"" or state = null`
* `state = ""WI"" and city = ""Milwaukee""`
* `(state = ""WI"" or state = ""CA"") and postalCode != null`
* `isActive = true`
* `city.StartsWith(""Mil"")`
* `city.EndsWith(""ton"")`
* `city.Contains(""Mil"")`
* `city.EndsWith(""ton"") and state = ""MA""`

The filter can also perform a LIKE operations like this:
* `DbFunctionsExtensions.Like(EF.Functions, City, ""mil%"")`

Note that not all fields can be filtered.

## Ordering Results

Results can be ordered with the `order` parameter. This is a comma-separted list of fields to sort on. Each field can have an optional `asc` or `desc` afterwards to indicate direction. Here
are some examples:

* `state`
* `state desc, city asc`

Note that not all fields can be sorted.
"
                });

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    In = "header",
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = "apiKey"
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });

                c.OperationFilter<ResponseHeaderOperationFilter>();

                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "MkeAlerts.Web.xml"));
            });

            // In production, the VueJS files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddTransient<IEntityReadService<ApplicationUser, Guid>, ApplicationUserService>();
            services.AddTransient<IEntityReadService<Property, string>, PropertyService>();
            services.AddTransient<IEntityWriteService<Property, string>, PropertyService>();

            services.AddSingleton<IValidator<Property>, PropertyValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHangfireDashboard();

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Milwaukee Alerts API - V1");
            });

            app.UseAuthentication();

            // This must be before UseMVC
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    //spa.UseReactDevelopmentServer(npmScript: "start");

                    // run npm process with client app
                    spa.UseVueCli(npmScript: "serve", port: 5020);
                    // if you just prefer to proxy requests from client app, use proxy to SPA dev server instead,
                    // app should be already running before starting a .NET client:
                    // spa.UseProxyToSpaDevelopmentServer("http://localhost:8080"); // your Vue app port                    
                }
            });

            dbContext.Database.EnsureCreated();
        }
    }
}
