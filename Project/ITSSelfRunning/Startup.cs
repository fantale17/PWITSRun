using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ITSSelfRunning.Data;
using ITSSelfRunning.Models;
using ITSSelfRunning.Services;
using Lib.Repos;
using Lib.Repos.Interfaces;
using Microsoft.Azure.EventHubs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ITSSelfRunning
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            var cs = Configuration["ConnectionStrings:DefaultConnection"];

            //cs for eventHub
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(Configuration["ConnectionStrings:EventHubsCs"])
            {
                EntityPath = "runnerHub1"

            };
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddSingleton<IActivityRepo> (new ActivityRepo(cs));
            services.AddSingleton<IPointRepo>(new PointRepo(cs));
            services.AddSingleton<IEventHubService>(new EventHubService(connectionStringBuilder));
            services.AddSingleton<IRunnerRepo>(new RunnerRepo(cs));
            services.AddSingleton<ICloudStorageService>(new CloudStorageService(Configuration["ConnectionStrings:StorageConnectionString"]));
            services.AddSingleton<ISharedRunService>(new SharedRunService(Configuration["SharedRunUrl"]));
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
