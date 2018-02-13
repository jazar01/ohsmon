using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ohsmon.Models;
using Microsoft.EntityFrameworkCore;

namespace ohsmon
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
            services.AddEntityFrameworkNpgsql().AddDbContext<MonitorContext>(opt => 
                opt.UseNpgsql(Configuration.GetConnectionString("ohsmondb")));
            services.AddMvc();
            services.Configure<IISOptions>(options =>
            {
                options.AutomaticAuthentication = true;
                options.AuthenticationDisplayName = null;
                options.ForwardClientCertificate = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}

/* NOTES
 * 
 *  Make sure Postgress is already installed and make an admin user ID and Pwd for connect string (must be full admin)
 *  
 *  in appsettings.json file
 *  
 *    "ConnectionStrings": {
 *           "ohsmondb": "User ID=monroot;Password=1234;Host=localhost;Port=5432;Database=ohsmonitor;Pooling=true;"
 *     }
 *  
 *  Create inital databaes using EF Migration
 *  
 *  Edit csproj file  (otherwise you will get "dotnet-ef" not found msg.
 *  USE THIS
 *     <DotNetCliToolReference Include = "Microsoft.EntityFrameworkCore.Tools.DotNet" Version="2.0.0" />
 *  NOT THIS
 *     <PackageReference Include="Microsoft.EntityFrameworkCore.Tools.DotNet" Version="2.0.0" />
 * 
 *  Powershell from same dir as csproj file
 *      dotnet ef migrations add IntialMigration
 *      dotnet ef database update
 *  
 *  Postgress db with table should be defined now
 *  
 *  Useful 11 min video   https://www.youtube.com/watch?v=md20lQut9EE
 *  
 *  Use of Dotnet EF migration commands https://benjii.me/2016/05/dotnet-ef-migrations-for-asp-net-core/
 *  
 */

