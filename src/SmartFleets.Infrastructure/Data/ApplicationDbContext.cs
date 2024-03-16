using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartFleets.Domain.Entities;

#nullable disable

namespace SmartFleets.Infrastructure.Data
{
    /// <summary>
    /// Represents the application's database context.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        private readonly IServiceProvider _services;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="services">The service provider for accessing configuration and other services.</param>
        public ApplicationDbContext(IServiceProvider services)
        {
            _services = services;
        }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> for <see cref="Fault"/> entities.
        /// </summary>
        public DbSet<Fault> Faults { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> for <see cref="FaultMetadata"/> entities.
        /// </summary>
        public DbSet<FaultMetadata> FaultMetadatas { get; set; }

        /// <summary>
        /// Configures the database context.
        /// </summary>
        /// <param name="optionsBuilder">The options builder for configuring the context.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _services.GetService<IConfiguration>().GetConnectionString("smartfleets_db");
            DbContextConfiguringValidator.Validate(connectionString);
            optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("SmartFleets.Infrastructure"));
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> to ensure <see cref="DbContext.SaveChangesAsync(CancellationToken)"/> is used instead of <see cref="SaveChanges"/>.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">A value indicating whether all changes should be accepted on success.</param>
        /// <exception cref="InvalidOperationException">Always thrown to enforce the use of <see cref="DbContext.SaveChangesAsync(CancellationToken)"/>.</exception>
        [Obsolete("Always use SaveChangesAsync")]
#pragma warning disable CS0809
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
#pragma warning restore CS0809
        {
            throw new InvalidOperationException("Always use SaveChangesAsync. Calling SaveChanges spins up new thread and wastes server resources.");
        }
    }
}
