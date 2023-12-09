using Cnab.Application.Database;
using Cnab.Application.Jobs;
using Cnab.Application.Repository;
using Cnab.Application.Services;
using Cnab.Application.UoW;
using Cnab.Domain.Repository;
using Cnab.Domain.Services;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System.Data;

namespace Cnab.Application;

public static class Configure
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services, IConfiguration configuration) =>
        services.InjectDependences()
                .InjectDatabase(configuration)
                .InjectQuartzJobs(configuration);

    public static IServiceCollection InjectQuartzJobs(this IServiceCollection services, IConfiguration configuration)
    {
        var minutes = int.TryParse(configuration["ProccessCnabJob:IntervalMinutes"], out var result) ? result : 10;

        services.AddQuartz(c =>
        {
            var jobKey = new JobKey(nameof(ProccessCnabItemsJob));

            c.AddJob<ProccessCnabItemsJob>(jobKey)
                .AddTrigger(trigger => trigger.ForJob(jobKey)
                    .WithSimpleSchedule(schedules => schedules.WithIntervalInMinutes(minutes).RepeatForever()));

        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        return services;
    }

    public static IServiceCollection InjectDependences(this IServiceCollection services)
    {
        services.AddScoped<UnitOfWork>();

        services.AddScoped<IImportRepository, ImportRepository>();
        services.AddScoped<IStoreRepository, StoreRepository>();
        services.AddScoped<IImportServices, ImportServices>();
        services.AddScoped<IProcessCnabServices, ProcessCnabServices>();
        services.AddScoped<IStoreServices, StoreServices>();

        return services;
    }

    public static IServiceCollection InjectDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("SQLiteConnection") ?? string.Empty;

        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString, nameof(connectionString));
        services.AddScoped<IDbConnection>(provider => new SqliteConnection(connectionString));

        new DatabaseInitializer(new SqliteConnection(connectionString)).Initialize();
        return services;
    }
}
