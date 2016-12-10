using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Redakt.Core.Extensions;
using Redakt.Data;

namespace Redakt.Starter.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RedaktSettings>(Configuration.GetSection("RedaktSettings"));

            //services.AddLogging();

            services.AddMvc(options =>
            {
                //options.InputFormatters.Clear();

                //var serializerSettings = JsonSerializerSettingsProvider.CreateSerializerSettings();
                //serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                //serializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore;

                //var jsonOutputFormatter = new JsonOutputFormatter(serializerSettings);

                //options.OutputFormatters.RemoveType<JsonOutputFormatter>(); //.RemoveAll(formatter => formatter.Instance.GetType() == typeof(JsonOutputFormatter));
                //options.OutputFormatters.Insert(0, jsonOutputFormatter);
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", cors => cors.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            services.AddRedakt(options =>
            {
                options.UseTestData = false;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //app.UseIISPlatformHandler();

            app.UseStaticFiles();

            app.UseCors("AllowAll");

            app.UseRedakt();

            app.UseMvc();
        }
    }
}
