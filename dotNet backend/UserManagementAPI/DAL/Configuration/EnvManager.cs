namespace UserManagementAPI.DAL.Configuration
{

    public enum EnvTypes
    {
        DB_CONNECTION_STRING,
        Jwt_Key,
        Jwt_Issuer,
        Jwt_Audience
    }

    public static class EnvManager
    {
        public static void CheckRequiredEnvironmentVariables()
        {
            foreach (EnvTypes envVar in Enum.GetValues(typeof(EnvTypes)))
            {
                var value = Environment.GetEnvironmentVariable(envVar.ToString());
                if (string.IsNullOrEmpty(value))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine($"❌ ERROR: Environment variable '{envVar}' is missing.");
                    Console.ResetColor();
                    throw new InvalidOperationException($"Environment variable '{envVar}' is missing.");
                }
            }
        }

        public static string GetEnvVariable(EnvTypes envType)
        {
            return Environment.GetEnvironmentVariable(envType.ToString()) ?? "";
        }
    }
}
