using CsvQueueProcessor.Core.Configuration;
using CsvQueueProcessor.Core.Interfaces;
using CsvQueueProcessor.Core.Services;
using CsvQueueProcessor.Infrastructure.Repositories;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
// Bind RabbitMQ configuration from appsettings.json
builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMq"));

// Register the RabbitMQ service as a singleton
builder.Services.AddSingleton<IRabbitMqService>(provider =>
{
    var rabbitMqConfig = provider.GetRequiredService<IOptions<RabbitMqConfiguration>>().Value;
    return new RabbitMqService(rabbitMqConfig);
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMvc();
// Register repositories with DI container
builder.Services.AddScoped<IUserRepository>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new UserRepository(connectionString);
});
builder.Services.AddScoped<IFileProcessingRepository>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new FileProcessingRepository(connectionString);
});

// Register services with DI container
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFileProcessingService, FileProcessingService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Register repositories with DI container

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}");

app.Run();
