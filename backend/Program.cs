using Security_Service_AspNetCore;
using SecurityService_AspNetCore.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Конфигурация из appsettings.json
var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .Build();

var options = new ProjectOptions();
config.GetSection("Options").Bind(options);

builder.Services.AddExtensions(options);

// Add services to the container.
//builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("DefaultPolicyWithOrigin");

if (app.Environment.IsDevelopment() 
    || app.Environment.IsProduction() // MARK: Можно отключить, если в проде сваггер не нужен
    )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
