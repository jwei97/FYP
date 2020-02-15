using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FYP.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FYP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // peanut start
            //CreateWebHostBuilder(args).Build().Run();
            var host = CreateWebHostBuilder(args).Build();
            
            //make scope of running processes
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try //link with method process and make error handling
                {
                    var context = services.GetRequiredService<FYPContext>();
                    context.Database.Migrate();

                    //peanut
                    SeedData.Initialize(services);
                }
                catch (Exception Ex) // make a error log if error
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(Ex, "An error occured seeding the DB!");
                }
                host.Run();
            }

            // peanut end
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
