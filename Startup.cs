using MkeAlerts.Web.Data;
using AutoMapper;
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
using MkeAlerts.Web.Utilities;
using MkeAlerts.Web.Services.Data.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.OpenApi.Models;
using MkeAlerts.Web.Models.Data.AppHealth;
using Hangfire.Dashboard;
using Hangfire.Dashboard.BasicAuthorization;
using Serilog;

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
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default"), x =>
            {
                x.UseNetTopologySuite();
                x.CommandTimeout(600);
            }));

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
                .AddNewtonsoftJson(options =>
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
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Milwaukee Alerts API",
                    Version = "v1",
                    Description = @"
## Using the API

This API is designed for public consumption -- feel free to use it in your own application. I want this to be easily usable by other civic-oriented applications; accordingly, there aren't any API
keys or registration process for applications that want to use this API. However, because I manage this on my own (and fund it out of my own pocket), please:

* __Let me know if you're using the API.__ Send an email to cwilson at mkealerts.com and let me know what you're doing with it. This will help me keep track of what's being used, and also allow me to contact you if there might be any breaking changes coming. Also, I'm just interested in seeing any cool projects that use this!
* __Keep consumption to reasonable levels.__ To keep the cost manageable on this site, the underlying infrastructure is fairly limited. Don't do anything that will hog all the resources or prevent this site (or others using the API) from working.
* __Don't charge for anything that's using the API.__ The data in this API is from public sources, and I'm making it all available free of charge.

If you have any questions about how the API works, feel free to contact me. If you're interested in making changes/improvements, visit the project on GitHub: https://github.com/ChrisP1118/mkealerts.

## Authentication

Some of these API endpoints are only available to authenticated users. To authenticate, make a call to `GET /api/account/login`. Assuming valid credentials are provided, a JWT token will be
returned. This token should be included in the `Authorization` header as a bearer token on subsequent requests.

Within Swagger UI, you can click the 'Authorize' button to use a token on requests. Make sure that the value entered includes the word `Bearer` before the token -- the value Swagger UI is
sending is the entire `Authorization` header.

## Including Related Data

The `includes` parameter on many of these calls lets you specify related objects to include in the results. The parameter is a comma-delimited list of related objects; the objects use dotted
notation. For instance, to include `parcel` properties when calling `/api/properties`, set `includes` to `parcel`. If you also wanted to include `commonParcel`, which is a property of `parcel`,
you'd set `includes` to `parcel,parcel.commonParcel`.

## Filtering Results

Results can be filtered with the `filter` parameter. You can use basic conditional operations, parentheses for grouping, null checks, and operators like AND and OR. Here are some examples
(using the `/api/policeDispatchCall` endpoint):
* `natureOfCall=""ENTRY""`
* `natureOfCall=""ENTRY"" or natureOfCall=""THEFT""`
* `geometry != null`
* `geometry = null`
* `(natureOfCall=""ENTRY"" or natureOfCall=""THEFT"") and geometry != null`
* `district = 1`

There are also a handful of functions you can use, including `StartsWith`, `EndsWith`, and `Contains`.
* `location.Contains(""LINCOLN"")`
* `location.Contains(""LINCOLN"") and !location.EndsWith("",MKE"")`

Note that not all fields can be filtered.

## Ordering Results

Results can be ordered with the `order` parameter. This is a comma-separted list of fields to sort on. Each field can have an optional `asc` or `desc` afterwards to indicate direction. Here
are some examples:

* `callNumber`
* `reportedDateTime desc`
* `district desc, location asc`

Note that not all fields can be sorted.

## Property Names

As much as possible, this API attempts to stick to property names used in the original data sources. This often leads to names that are snake-cased, rather than camelCased as you'd expect for JSON
data. It's also difficult to decipher the meaning of some of them. However, the goal is that it should make it easier for you to correlate them to the original data and determine their use and
meaning from documentation with the original data sources.

## Data Sources

Here are the original sources for the data exposed through this API. Additional documentation/explanations of the raw data is available here:
* `/api/address`: https://data.milwaukee.gov/dataset/mai
* `/api/commonParcel`: https://data.milwaukee.gov/dataset/parcel-outlines
* `/api/crime`: https://data.milwaukee.gov/dataset/wibr
* `/api/fireDispatchCall`: https://itmdapps.milwaukee.gov/MilRest/mfd/calls
* `/api/parcel`: https://data.milwaukee.gov/dataset/parcel-outlines
* `/api/policeDispatchCall`: https://itmdapps.milwaukee.gov/MPDCallData/index.jsp?district=All
* `/api/property`: https://data.milwaukee.gov/dataset/mprop
* `/api/street`: https://data.milwaukee.gov/dataset/streets
"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            },
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });

                //c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                //{
                //    In = "header",
                //    Description = "Please insert JWT with Bearer into field",
                //    Name = "Authorization",
                //    Type = "apiKey"
                //});

                //c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                //{
                //    { "Bearer", new string[] { } }
                //});

                c.OperationFilter<ResponseHeaderOperationFilter>();

                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "MkeAlerts.Web.xml"));
            });
            services.AddSwaggerGenNewtonsoftSupport();

            // In production, the VueJS files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddTransient<IEntityReadService<ApplicationUser, Guid>, ApplicationUserService>();
            services.AddTransient<IEntityReadService<Parcel, string>, ParcelService>();
            services.AddTransient<IEntityWriteService<Parcel, string>, ParcelService>();
            services.AddTransient<IParcelService, ParcelService>();
            services.AddTransient<IEntityReadService<CommonParcel, int>, CommonParcelService>();
            services.AddTransient<IEntityWriteService<CommonParcel, int>, CommonParcelService>();
            services.AddTransient<ICommonParcelService, CommonParcelService>();
            services.AddTransient<IEntityReadService<Address, int>, AddressService>();
            services.AddTransient<IEntityWriteService<Address, int>, AddressService>();
            services.AddTransient<IEntityReadService<Street, int>, StreetService>();
            services.AddTransient<IEntityWriteService<Street, int>, StreetService>();
            services.AddTransient<IEntityReadService<Property, Guid>, PropertyService>();
            services.AddTransient<IEntityWriteService<Property, Guid>, PropertyService>();
            services.AddTransient<IPropertyService, PropertyService>();
            services.AddTransient<IEntityReadService<PoliceDispatchCall, string>, PoliceDispatchCallService>();
            services.AddTransient<IEntityWriteService<PoliceDispatchCall, string>, PoliceDispatchCallService>();
            services.AddTransient<IEntityReadService<PoliceDispatchCallType, string>, PoliceDispatchCallTypeService>();
            services.AddTransient<IEntityWriteService<PoliceDispatchCallType, string>, PoliceDispatchCallTypeService>();
            services.AddTransient<IEntityReadService<FireDispatchCall, string>, FireDispatchCallService>();
            services.AddTransient<IEntityWriteService<FireDispatchCall, string>, FireDispatchCallService>();
            services.AddTransient<IEntityReadService<FireDispatchCallType, string>, FireDispatchCallTypeService>();
            services.AddTransient<IEntityWriteService<FireDispatchCallType, string>, FireDispatchCallTypeService>();
            services.AddTransient<IEntityReadService<Crime, string>, CrimeService>();
            services.AddTransient<IEntityWriteService<Crime, string>, CrimeService>();
            services.AddTransient<IEntityReadService<DispatchCallSubscription, Guid>, DispatchCallSubscriptionService>();
            services.AddTransient<IEntityWriteService<DispatchCallSubscription, Guid>, DispatchCallSubscriptionService>();
            services.AddTransient<IEntityReadService<PickupDatesSubscription, Guid>, PickupDatesSubscriptionService>();
            services.AddTransient<IEntityWriteService<PickupDatesSubscription, Guid>, PickupDatesSubscriptionService>();
            services.AddTransient<IEntityReadService<JobRun, Guid>, JobRunService>();
            services.AddTransient<IEntityWriteService<JobRun, Guid>, JobRunService>();
            services.AddTransient<IJobRunService, JobRunService>();

            services.AddTransient<IStreetReferenceService, StreetReferenceService>();
            services.AddTransient<IGeocodingService, GeocodingService>();
            services.AddTransient<IMailerService, MailjetMailerService>();
            services.AddTransient<IPickupDatesService, PickupDatesService>();

            services.AddSingleton<IValidator<Address>, AddressValidator>();
            services.AddSingleton<IValidator<Parcel>, ParcelValidator>();
            services.AddSingleton<IValidator<CommonParcel>, CommonParcelValidator>();
            services.AddSingleton<IValidator<Street>, StreetValidator>();
            services.AddSingleton<IValidator<Property>, PropertyValidator>();
            services.AddSingleton<IValidator<PoliceDispatchCall>, PoliceDispatchCallValidator>();
            services.AddSingleton<IValidator<PoliceDispatchCallType>, PoliceDispatchCallTypeValidator>();
            services.AddSingleton<IValidator<FireDispatchCall>, FireDispatchCallValidator>();
            services.AddSingleton<IValidator<FireDispatchCallType>, FireDispatchCallTypeValidator>();
            services.AddSingleton<IValidator<Crime>, CrimeValidator>();
            services.AddSingleton<IValidator<DispatchCallSubscription>, DispatchCallSubscriptionValidator>();
            services.AddSingleton<IValidator<PickupDatesSubscription>, PickupDatesSubscriptionValidator>();
            services.AddSingleton<IValidator<JobRun>, JobRunValidator>();
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

            dbContext.Database.Migrate();

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { 
                    new BasicAuthAuthorizationFilter(
                        new BasicAuthAuthorizationFilterOptions
                        {
                            RequireSsl = false,
                            SslRedirect = false,
                            LoginCaseSensitive = true,
                            Users = new []
                            {
                                new BasicAuthAuthorizationUser
                                {
                                    Login = Configuration["HangfireDashboardUsername"],
                                    PasswordClear =  Configuration["HangfireDashboardPassword"]
                                }
                            }

                        }) }
            });

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Milwaukee Alerts API - V1");
            });

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // This must be before UseMVC
            app.UseMiddleware<ExceptionMiddleware>();

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller}/{action=Index}/{id?}");
            //});
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller}/{action=Index}/{id?}");


                if (env.IsDevelopment())
                {
                    // NOTE: VueCliProxy is meant for developement and hot module reload
                    // NOTE: SSR has not been tested
                    // Production systems should only need the UseSpaStaticFiles() (above)
                    // You could wrap this proxy in either
                    // if (System.Diagnostics.Debugger.IsAttached)
                    // or a preprocessor such as #if DEBUG
                    endpoints.MapToVueCliProxy(
                        "{*path}",
                        new SpaOptions { SourcePath = "ClientApp" },
                        //npmScript: (System.Diagnostics.Debugger.IsAttached) ? "serve" : null,
                        regex: "Compiled successfully",
                        forceKill: true
                    );
                }
            });

            //app.UseSpa(spa =>
            //{
            //    spa.Options.SourcePath = "ClientApp";

            //    if (env.IsDevelopment())
            //    {
            //        //spa.UseReactDevelopmentServer(npmScript: "start");

            //        // run npm process with client app
            //        spa.UseVueCli(npmScript: "serve", port: 5020);
            //        // if you just prefer to proxy requests from client app, use proxy to SPA dev server instead,
            //        // app should be already running before starting a .NET client:
            //        // spa.UseProxyToSpaDevelopmentServer("http://localhost:8080"); // your Vue app port                    
            //    }
            //});

            //dbContext.Database.EnsureCreated();

            //// Run every 15 minutes
            //RecurringJob.AddOrUpdate<HealthCheckJob>(x => x.Run(), "*/15 * * * *");

            // Run every 5 minutes
            RecurringJob.AddOrUpdate<ImportPoliceDispatchCallsJob>(x => x.Run(), "*/5 * * * *");

            // Run every 5 minutes
            RecurringJob.AddOrUpdate<ImportFireDispatchCallsJob>(x => x.Run(), "*/5 * * * *");

            //// Run at 10am and 10pm
            RecurringJob.AddOrUpdate<UpdateNextPickupDatesNotifications>(x => x.Run(), "0 10,22 * * *");

            //// Run every hour
            RecurringJob.AddOrUpdate<SendPickupDatesNotifications>(x => x.Run(), "0 * * * *");

            if (!env.IsDevelopment())
            {
                // Every day at 6:00am UTC (Dataset is updated daily: https://data.milwaukee.gov/dataset/wibr)
                RecurringJob.AddOrUpdate<ImportCrimesJob>("ImportCrimesJob", x => x.Run(), "0 6 * * *");

                // First day of every month at 1:00am UTC - this dataset shouldn't really change
                RecurringJob.AddOrUpdate<ImportCrimesArchiveJob>("ImportCrimesArchiveJob", x => x.Run(), "0 1 1 * *");

                // Every Saturday and Wednesday at 9:00am UTC
                RecurringJob.AddOrUpdate<ImportPropertiesJob>("ImportPropertiesJob", x => x.Run(), "0 9 * * SAT,WED");
            }
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
                for (int i = 0; i < array.Length && (i != 1 || char.IsUpper(array[i]) || (i == 1 && array[i] == '_')); i++)
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
