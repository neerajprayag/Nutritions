using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Nutritions.Utility;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
//using Microsoft.AspNetCore.Mvc.NewtonsoftJson;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpClient(); // Register IHttpClientFactory

// Configure DbContext
//var connectionString = builder.Configuration.GetConnectionString("NutritionDb");
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<NutritionDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Add CORS
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowReactApp", policy =>
//    {
//        policy.WithOrigins(
//            "http://localhost:3000",
//            "https://neerajprayag.github.io"
//            )
//              .AllowAnyHeader()
//              .AllowAnyMethod();
//    });
//});
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {

            // policy.WithOrigins("https://agreeable-bay-093c66d0f.6.azurestaticapps.net")
            policy.WithOrigins(
            "http://localhost:3000",
            "https://agreeable-bay-093c66d0f.6.azurestaticapps.net"
        )
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});



// Add Controllers
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

    


var app = builder.Build();

// Configure middleware
//comment out the below code to disable swagger in production
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication(); // Ensure this middleware is included
app.UseAuthorization();
app.UseCors("AllowReactApp");

app.MapControllers();

app.Run();
