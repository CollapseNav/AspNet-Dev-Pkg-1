using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using System.Linq;

namespace AspNet.Dev.Pkg.Demo
{
    public class DemoDbContext : DbContext
    {
        public DbSet<Demo> Demos { get; set; }
        public DbSet<DemoAgain> DemoAgains { get; set; }

        public DemoDbContext(DbContextOptions<DemoDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ConfigDataBase();
        }
    }

    public static class DataExtensions
    {
        public static void ConfigDataBase(this ModelBuilder builder)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes()
                        .Where(t =>
                        {
                            return t.GetInterfaces().Any(i => i.IsGenericType
                             && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));
                        }).Select(t => Activator.CreateInstance(t));
            foreach (dynamic item in types)
            {
                builder.ApplyConfiguration(item);
            }
        }
    }
}
