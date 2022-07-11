// TODO: Handle long long report responding.

// TODO: Add global exception handler with pretty errorModel type

// TODO: Check if path is too long
// TODO: Check if path is valid 


using CheckMyDirs.IoC;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


// Add custom services
builder.Services.ConfigureCors();

// Add handlers
builder.Services.AddHandlerServices();


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsProduction())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.MapControllers();

app.Run();