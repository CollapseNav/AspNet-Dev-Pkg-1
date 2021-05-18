using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using System.Linq;
using AspNet.Dev.Pkg.Infrastructure.Interface;
using System.Linq.Expressions;

namespace AspNet.Dev.Pkg.Demo
{
    public class DemoDbContext : DbContext
    {
        public DbSet<Demo> Demos { get; set; }
        public DbSet<DemoAgain> DemoAgains { get; set; }
        public DbSet<Test> Tests { get; set; }

        public DemoDbContext(DbContextOptions<DemoDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            bool IsDeleted = true;
            foreach (var entityType in builder.Model.GetEntityTypes().Where(e => typeof(IBaseEntity).IsAssignableFrom(e.ClrType)))
            {
                builder.Entity(entityType.ClrType).Property<bool>("IsDeleted");
                var parameter = Expression.Parameter(entityType.ClrType, "entity");
                var body = Expression.NotEqual(Expression.Call(typeof(EF), nameof(EF.Property), new[] { typeof(bool) }, parameter, Expression.Constant("IsDeleted")), Expression.Constant(IsDeleted));

                builder.Entity(entityType.ClrType).HasQueryFilter(Expression.Lambda(body, parameter));
            }
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
