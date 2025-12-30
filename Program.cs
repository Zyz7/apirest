
using ApiRest.repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Context
/*
builder.Services.AddDbContext<Context>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DBConnection"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("DBConnection")
        )
    )
);
*/
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlite(
        "Data Source=/data/apirest.db"
    )
);

// Jwt
var secretKey = builder.Configuration["Jwt:Key"];
var issuer = builder.Configuration["Jwt:Issuer"];
var audience = builder.Configuration["Jwt:Audience"];
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey ?? string.Empty))
    };
});

// Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowEverything", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });

    options.AddPolicy("AllowDomain", policy =>
    {
        policy.WithOrigins("http://localhost")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "ApiRest C#",
        Description = "Example of api with c#, database, jwt, swagger in .net 9",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "zyz7",
            Email = "zyz7@pm.me"
        }
    });   

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    { 
        Name = "Authorization", 
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey, 
        Scheme = "Bearer", 
        BearerFormat = "JWT", 
        In = Microsoft.OpenApi.Models.ParameterLocation.Header, 
        Description = "Enter the JWT token in the format: Bearer {token}"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    { 
        { new Microsoft.OpenApi.Models.OpenApiSecurityScheme 
            { 
                Reference = new Microsoft.OpenApi.Models.OpenApiReference 
                    { 
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme, 
                        Id = "Bearer" 
                    } 
            }, 
            new List<string>() 
        } 
    });

});

// Port
var port = Environment.GetEnvironmentVariable("PORT") ?? "5054";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Controllers
builder.Services.AddAuthorization();
builder.Services.AddControllers();

var app = builder.Build();

//if (app.Environment.IsDevelopment() )
//{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiRest C#");
        options.RoutePrefix = string.Empty; //swagger in the root
    });
//}

app.UseCors("AllowEverything");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
