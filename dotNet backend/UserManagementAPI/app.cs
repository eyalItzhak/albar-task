using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using UserManagementAPI.BL.Services;
using UserManagementAPI.BL.Validators;
using UserManagementAPI.DAL.Configuration;
using UserManagementAPI.Middlewares;

namespace UserManagementAPI
{
    public class App
    {
        public static WebApplication CreateApp(string[] args)
        {
            DotNetEnv.Env.Load();
            var builder = WebApplication.CreateBuilder(args);
            EnvManager.CheckRequiredEnvironmentVariables();
            DatabaseConfiguration.ConfigureDatabase(builder);

            RegisterServices(builder);
            ConfigureControllers(builder);
            ConfigureValidators(builder);
            var app = builder.Build();
            AddMiddlewares(app);
            app.MapControllers();
            return app;
        }

        private static void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IJwtService>(serviceProvider =>
                new JwtService(
                    EnvManager.GetEnvVariable(EnvTypes.Jwt_Key),
                    EnvManager.GetEnvVariable(EnvTypes.Jwt_Issuer),
                    EnvManager.GetEnvVariable(EnvTypes.Jwt_Audience)
                ));

            builder.Services.AddSingleton<IPasswordService, PasswordService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IProductService, ProductService>();

            // Add CORS services
            ConfigureCORS(builder.Services);  // Add this line to register CORS configuration
        }

        private static void ConfigureCORS(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    builder => builder
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
        }

        private static void ConfigureControllers(WebApplicationBuilder builder)
        {
            builder.Services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
        }

        private static void ConfigureValidators(WebApplicationBuilder builder)
        {
            builder.Services.AddValidatorsFromAssembly(typeof(UpdateProductDtoValidator).Assembly);
            builder.Services.AddValidatorsFromAssembly(typeof(ProductDtoValidator).Assembly);
            builder.Services.AddValidatorsFromAssembly(typeof(AuthDtoValidator).Assembly);
            builder.Services.AddFluentValidationAutoValidation();
        }

        private static void AddMiddlewares(WebApplication app)
        {
            app.UseCors("AllowFrontend");
            app.UseMiddleware<JwtMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
