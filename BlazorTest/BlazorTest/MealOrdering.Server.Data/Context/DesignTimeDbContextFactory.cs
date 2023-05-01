using MealOrdering.Server.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealOrdering.Server.Data.Context
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MealOrderinDbContext>
    {
        public MealOrderinDbContext CreateDbContext(string[] args)
        {
            string connectionString = "Database";
            var builder = new DbContextOptionsBuilder<MealOrderinDbContext>();
            builder.UseNpgsql(connectionString);
            return new MealOrderinDbContext(builder.Options);
        }
    }
}
