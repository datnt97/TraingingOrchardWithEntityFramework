using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace eTweb.Data.EF
{
    public class eTwebContextFactory : IDesignTimeDbContextFactory<eTwebDbContext>
    {
        public eTwebDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionStrings = configuration.GetConnectionString("eTwebConnectionDb");

            var optionsBuilder = new DbContextOptionsBuilder<eTwebDbContext>();
            optionsBuilder.UseSqlServer(connectionStrings);

            return new eTwebDbContext(optionsBuilder.Options);
        }
    }
}
