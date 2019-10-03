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
using MkeAlerts.Web.Models.Data.Places;
using MkeAlerts.Web.Models.Data.Incidents;
using MkeAlerts.Web.Jobs;
using NetTopologySuite.IO.Converters;
using MkeAlerts.Web.Services.Functional;
using MkeAlerts.Web.Models.Data.Subscriptions;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Routing;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Globalization;

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

            services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(Configuration.GetConnectionString("Default"));
            });
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
            services
                .AddMvc(options =>
                {
                    // Use camelCase in URLs and routing
                    options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Formatting = Formatting.Indented;

                    // Use camelCase on DTOs
                    //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver()
                    {
                        NamingStrategy = new CustomCamelCaseNamingStrategy()
                        {
                            ProcessDictionaryKeys = true,
                            OverrideSpecifiedNames = true
                        }
                    };
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

                    // This is required for NetTopologySuite.IO.Converters.GeometryConverter to work correctly
                    options.SerializerSettings.Converters.Add(new CoordinateConverter());
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
            services.AddTransient<IEntityReadService<Parcel, string>, ParcelService>();
            services.AddTransient<IEntityWriteService<Parcel, string>, ParcelService>();
            services.AddTransient<IEntityReadService<Address, string>, AddressService>();
            services.AddTransient<IEntityWriteService<Address, string>, AddressService>();
            services.AddTransient<IEntityReadService<Street, string>, StreetService>();
            services.AddTransient<IEntityWriteService<Street, string>, StreetService>();
            services.AddTransient<IEntityReadService<PoliceDispatchCall, string>, PoliceDispatchCallService>();
            services.AddTransient<IEntityWriteService<PoliceDispatchCall, string>, PoliceDispatchCallService>();
            services.AddTransient<IEntityReadService<FireDispatchCall, string>, FireDispatchCallService>();
            services.AddTransient<IEntityWriteService<FireDispatchCall, string>, FireDispatchCallService>();
            services.AddTransient<IEntityReadService<Crime, string>, CrimeService>();
            services.AddTransient<IEntityWriteService<Crime, string>, CrimeService>();
            services.AddTransient<IEntityReadService<DispatchCallSubscription, Guid>, DispatchCallSubscriptionService>();
            services.AddTransient<IEntityWriteService<DispatchCallSubscription, Guid>, DispatchCallSubscriptionService>();

            services.AddTransient<IStreetReferenceService, StreetReferenceService>();

            services.AddTransient<IGeocodingService, GeocodingService>();

            services.AddSingleton<IValidator<Property>, PropertyValidator>();
            services.AddSingleton<IValidator<Address>, AddressValidator>();
            services.AddSingleton<IValidator<Parcel>, ParcelValidator>();
            services.AddSingleton<IValidator<Street>, StreetValidator>();
            services.AddSingleton<IValidator<PoliceDispatchCall>, PoliceDispatchCallValidator>();
            services.AddSingleton<IValidator<FireDispatchCall>, FireDispatchCallValidator>();
            services.AddSingleton<IValidator<Crime>, CrimeValidator>();
            services.AddSingleton<IValidator<DispatchCallSubscription>, DispatchCallSubscriptionValidator>();
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

            //BackgroundJob.Enqueue<ImportAddressesJob>(x => x.Run());

            // Run every 5 minutes
            RecurringJob.AddOrUpdate<ImportPoliceDispatchCallsJob>(x => x.Run(), "*/5 * * * *");

            // Run every 5 minutes
            RecurringJob.AddOrUpdate<ImportFireDispatchCallsJob>(x => x.Run(), "*/5 * * * *");

            /*
            // Every day at 1:00am (Dataset is updated daily: https://data.milwaukee.gov/dataset/mai)
            RecurringJob.AddOrUpdate<ImportAddressesJob>("ImportAddressesJob", x => x.Run(), "0 1 * * *");

            // Every day at 3:00am (Dataset is updated daily: https://data.milwaukee.gov/dataset/mprop)
            RecurringJob.AddOrUpdate<ImportPropertiesJob>("ImportPropertiesJob", x => x.Run(), "0 3 * * *");

            // Every day at 5:00am (Dataset is updated daily: https://data.milwaukee.gov/dataset/wibr)
            RecurringJob.AddOrUpdate<ImportCrimesJob>("ImportCrimesJob", x => x.Run(), "0 5 * * *");

            // Every day at 6:00pm on Sundays
            RecurringJob.AddOrUpdate<ImportParcelsJob>("ImportParcelsJob", x => x.Run(), "0 18 * * SUN");
            //BackgroundJob.Enqueue<ImportParcelsJob>(x => x.Run());

            // Every day at 9:00pm on Sundays
            RecurringJob.AddOrUpdate<ImportStreetsJob>("ImportStreetsJob", x => x.Run(), "0 21 * * SUN");
            */
        }
    }

    /// <summary>
    /// We want to use camelCased route names. There's a built-in option to use lowercase URLs, but we want camel casing.
    /// 
    /// Based on code from https://stackoverflow.com/questions/40334515/automatically-generate-lowercase-dashed-routes-in-asp-net-core
    /// </summary>
    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object value)
        {
            if (value == null)
                return null;

            // Slugify value
            //return Regex.Replace(value.ToString(), "([a-z])([A-Z])", "$1-$2").ToLower();
            string v = value.ToString();
            return v[0].ToString().ToLower() + v.Substring(1);
        }
    }

    /// <summary>
    /// The default camel case naming strategy doesn't work well with some of our property names (from external data) that are all uppercase with underscores. This uses a strategy that's more in
    /// line with the fields we have.
    /// 
    /// Based on code from https://stackoverflow.com/questions/52374261/issue-with-default-camelcase-serialization-of-all-caps-property-names-to-json-in
    /// </summary>
    public class CustomCamelCaseNamingStrategy : CamelCaseNamingStrategy
    {
        protected override String ResolvePropertyName(String propertyName)
        {
            return this.ToCamelCase(propertyName);
        }

        private string ToCamelCase(string s)
        {
            if (!string.IsNullOrEmpty(s) && char.IsUpper(s[0]))
            {
                char[] array = s.ToCharArray();
                for (int i = 0; i < array.Length && (i != 1 || char.IsUpper(array[i])); i++)
                {
                    bool flag = i + 1 < array.Length;
                    if ((i > 0 & flag) && array[i + 1] != '_' && !char.IsUpper(array[i + 1]) && !char.IsNumber(array[i + 1]))
                    {
                        break;
                    }
                    char c = char.ToLower(array[i], CultureInfo.InvariantCulture);
                    array[i] = c;
                }
                return new string(array);
            }
            return s;
        }
    }
}
