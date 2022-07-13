using CheckMyDirs.Api.Contracts;
using CheckMyDirs.Api.Exceptions;
using CheckMyDirs.Api.Features;
using CheckMyDirs.Common.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace CheckMyDirs.Api.IoC;

public static class StartupServices
{
    public static void ConfigureLoggerService(this IServiceCollection services) =>
        services.AddSingleton<ILoggerManager, LoggerManager>();
    
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
            options.AddPolicy("CorsPolicy", policy =>
                policy.AllowAnyHeader()
                    .WithOrigins("https://localhost:7298", "https://localhost:5153") //TODO: add to environment variables and replace here
                    .AllowAnyMethod()));
    }
    
    public static void ConfigureExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async httpContext =>
            {
                httpContext.Response.ContentType = "application/json";
                
                var contextFeature = httpContext.Features.Get<IExceptionHandlerFeature>(); 
                
                if (contextFeature != null)
                {
                    httpContext.Response.StatusCode = contextFeature.Error switch {
                        InvalidPathException => StatusCodes.Status200OK,
                        InvalidPreviousStatesFileParsingException => StatusCodes.Status200OK,
                        _ => StatusCodes.Status200OK
                    };
                    
                    // send report about error
                    await httpContext.Response.WriteAsJsonAsync(new FinalReportType()
                    {   
                        Error = contextFeature.Error.Message,
                        Records = null,
                        Message = null
                        
                    });
                }
            });
        });
    }
    
    public static void AddHandlerServices(this IServiceCollection services)
    {
        services.AddSingleton<PathValidationHandler>();
        services.AddSingleton<PreviousFilesStatesHandler>();
        services.AddSingleton<CleaningMessHandler>();
        
        services.AddTransient<ProcessingHandler>();
        services.AddTransient<CurrentFilesStatesHandler>();
        services.AddTransient<ComparerHandler>();
    }
    
}