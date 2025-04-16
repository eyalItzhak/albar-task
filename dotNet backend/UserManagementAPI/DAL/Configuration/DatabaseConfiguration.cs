using Microsoft.EntityFrameworkCore;
using UserManagementAPI.DAL.Context;

namespace UserManagementAPI.DAL.Configuration
{
    public static class DatabaseConfiguration
    {
        public static void ConfigureDatabase(WebApplicationBuilder builder)
        {
            var connectionString = EnvManager.GetEnvVariable(EnvTypes.DB_CONNECTION_STRING);
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
        }
    }
}
