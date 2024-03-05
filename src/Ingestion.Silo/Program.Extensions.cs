#nullable disable

using StackExchange.Redis;

namespace Ingestion.Silo
{
    public partial class Program
    {
    }
}

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ProgramExtensions
    {
        /// <summary>
        /// Configures Redis as the default grain storage provider.
        /// </summary>
        /// <param name="silo">Silo configuring builder.</param>
        /// <param name="connectionString">Redis connection string to connect to.</param>
        /// <returns>The Silo builder instance with configurations applied to.</returns>
        public static ISiloBuilder AddRedisGrainStorageAsDefault(this ISiloBuilder silo, string connectionString)
        {
            return silo.AddRedisGrainStorageAsDefault(redis =>
            {
                redis.ConfigurationOptions = ConfigurationOptions.Parse(connectionString);
            });
        }
    }
}


