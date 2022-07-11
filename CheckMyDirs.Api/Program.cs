// TODO: Handle long long report responding.

// TODO: Change .pseudogit file location to the same as investigated folder and exclude .pseudogit !!

// TODO: Check if path is too long
// TODO: Check if path is valid 


using CheckMyDirs.Api.IoC;
using NLog;

var builder = WebApplication.CreateBuilder(args);

// Configure log manager
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(),
    "/nlog.config"));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add custom services
builder.Services.ConfigureCors();
builder.Services.ConfigureLoggerService();

// Add handlers
builder.Services.AddHandlerServices();


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsProduction())
{
    app.UseHsts();
}

app.ConfigureExceptionHandler();

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.MapControllers();

app.Run();