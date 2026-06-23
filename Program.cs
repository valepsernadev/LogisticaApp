using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using LogisticaApp.Data;
using LogisticaApp.Middlewares;
using LogisticaApp.Services;
using LogisticaApp.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// --- DbContext ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
           .UseSnakeCaseNamingConvention());

// --- Autenticación JWT ---
var jwtKey = builder.Configuration["Jwt:Key"]!;
var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;
var jwtAudience = builder.Configuration["Jwt:Audience"]!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// --- Autorización por roles ---
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Cliente", policy => policy.RequireRole("cliente"));
    options.AddPolicy("Conductor", policy => policy.RequireRole("conductor"));
    options.AddPolicy("Operador", policy => policy.RequireRole("operador"));
    options.AddPolicy("Administrador", policy => policy.RequireRole("administrador"));
});

// --- Services ---
builder.Services.AddScoped<IAuthService, AuthService>();

// --- Controllers y OpenAPI ---
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// --- Scalar (documentación de API) ---
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("LogisticaApp API");
        options.AddServer(new ScalarServer("http://localhost:8080"));
    }).AllowAnonymous();
}

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();