using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FGrid.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FGrid
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var serviceScope = host.Services.CreateScope();
            await using var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            
            await context.Database.MigrateAsync();

            await ApplicationDbContextSeed.SeedSampleData(context);
            
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}