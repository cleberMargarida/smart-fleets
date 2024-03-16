using System;

namespace SmartFleets.Infrastructure.Data
{
    /// <summary>
    /// Provides validation for database context configuration.
    /// </summary>
    internal static class DbContextConfiguringValidator
    {
        /// <summary>
        /// Validates the connection string and throws an exception if it is not configured properly.
        /// </summary>
        /// <param name="connectionString">The connection string to validate.</param>
        /// <exception cref="InvalidOperationException">Thrown if the connection string is null during certain EF Core Tools operations.</exception>
        public static void Validate(string connectionString)
        {
            string? command = null;

            if (connectionString is null)
            {
                if (Environment.StackTrace.Contains("Microsoft.EntityFrameworkCore.Tools.OperationExecutorBase.RemoveMigration"))
                {
                    command = "Remove-Migration";
                }

                if (Environment.StackTrace.Contains("Microsoft.EntityFrameworkCore.Tools.OperationExecutorBase.UpdateDatabase"))
                {
                    command = "Update-Database";
                }
            }

            if (!string.IsNullOrEmpty(command))
            {
                throw new InvalidOperationException($"""
                    The ConnectionString 'smartfleets_db' property has not been initialized.
                    The '{command}', command is not allowed when the ConnectionString is null.
                    To troubleshoot this issue you can try one of the following: 
                     - Add an connection string for 'SmartFleets.Api' at 'appsettings.json' containing your database at 'Data/Migrations'.
                     - Remove the migration manually by deleting the files and rolling back the ApplicationDbContextModelSnapshot. 
                     - Add another migration undoing the changes.
                    at SmartFleets.Infrastructure.Data.ApplicationDbContext.OnConfiguring(DbContextOptionsBuilder optionsBuilder)

                    """);
            }
        }
    }
}
