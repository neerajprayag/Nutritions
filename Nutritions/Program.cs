using Microsoft.EntityFrameworkCore;
using Nutritions.Utility;

var builder = WebApplication.CreateBuilder(args);

// Log the connection string for debugging
try
{
    var connectionString = builder.Configuration.GetConnectionString("NutritionDb");
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new Exception("Connection string 'NutritionDb' is not found or empty in appsettings.json.");
    }

    Console.WriteLine("Loaded Connection String: " + connectionString);

    builder.Services.AddDbContext<NutritionDbContext>(options =>
        options.UseSqlServer(connectionString));
}
catch (Exception ex)
{
    Console.WriteLine("Error configuring DbContext: " + ex.Message);
    throw;
}

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors("AllowReactApp");

try
{
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine("An error occurred while running the application: " + ex.Message);
    throw;
}
