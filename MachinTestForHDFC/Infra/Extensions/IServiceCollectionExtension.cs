using MachinTestForHDFC.Database;
using MachinTestForHDFC.Services;
using Microsoft.EntityFrameworkCore;
using NetCore.AutoRegisterDi;
using System.Reflection;

namespace PBA.Api.Infra.Extensions
{
    public static class IServiceCollectionExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            var assembliesToScan = new[]
            {
                Assembly.GetExecutingAssembly(),
                Assembly.GetAssembly(typeof(IBaseService))
            };

            services.RegisterAssemblyPublicNonGenericClasses(assembliesToScan)
                .Where(c => c.Name.EndsWith("Service"))
                .AsPublicImplementedInterfaces();
        }

        #region To register repositories
        //public static void RegisterRepositories(this IServiceCollection services)
        //{
        //    //services.AddTransient(typeof(ILoginRepository), typeof(LoginRepository));

        //    //services.AddTransient<SqlCommands>();
        //    //services.AddTransient<SqlServiceHelper>();
        //    //services.AddTransient<DapperServiceHelper>();
        //    //services.AddScoped<IJwtProvider, JwtProvider>();
        //    //services.AddTransient(typeof(FluentValidationActionFilter<>));
        //} 
        #endregion

        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TestDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlConnectionString"))
                        .EnableSensitiveDataLogging(true);
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddTransient<IUnitOfWork, UnitOfWork>();
        }

        #region To register validators and cors policy
        //public static IServiceCollection AddAllFluentValidators(this IServiceCollection services)
        //{
        //    //services.AddValidatorsFromAssemblyContaining<LoginValidator>();
        //    //services.AddScoped(typeof(FluentValidationActionFilter<>));
        //    //services.AddSingleton<IFilterProvider, FluentValidationFilterProvider>();
        //    //return services;
        //}

        //public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
        //{
        //    var origins = configuration.GetSection(Constants.APP_SETTINGS_KEY).GetValue<string>(Constants.CLIENT_APP_URL_KEY);

        //    string[]? urls = origins?.Split(",", StringSplitOptions.RemoveEmptyEntries);

        //    services.AddCors(c =>
        //    {
        //        c.AddPolicy(Constants.CORS_KEY, builder =>
        //            {
        //                builder
        //                //.WithOrigins(urls!)
        //                .AllowAnyOrigin()
        //                .AllowAnyHeader()
        //                .AllowAnyMethod();
        //            });
        //    });
        //} 
        #endregion
    }
}
