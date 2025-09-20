using System.Text;
using AutoMapper;
using eCommerce.API;
using eCommerce.API.Middlewares;
using eCommerce.Core;
using eCommerce.Core.Mappers;
using eCommerce.Core.Options;
using eCommerce.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------
// Services
// ----------------------------
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        // Configure JSON options here if needed
        // o.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.AddEndpointsApiExplorer();

// ✅ Swagger with Bearer JWT support
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "eCommerce API",
        Version = "v1",
        Description = "Clean Architecture API with JWT Auth"
    });

    var jwtScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Paste JWT here. You can include the `Bearer ` prefix or just the token."
    };

    c.AddSecurityDefinition("Bearer", jwtScheme);

    var requirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    };
    c.AddSecurityRequirement(requirement);
});

// Your layered registrations
builder.Services.AddCore(builder.Configuration);
builder.Services.AddInfrastruction(builder.Configuration);

// AutoMapper profiles
builder.Services.AddAutoMapper(typeof(ApplicationUserMappingProfile).Assembly);

// CORS (no cookies scenario)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
});
//builder.Services.AddCors(o => o.AddPolicy("web", p => {
//    p.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod();
//}));



// JWT Options binding
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
var jwtOpts = builder.Configuration.GetSection("Jwt").Get<JwtOptions>() ?? new JwtOptions();
var keyBytes = Encoding.UTF8.GetBytes(jwtOpts.SecretKey ?? string.Empty);

// Authentication + JWT Bearer
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // set true in production if behind HTTPS
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),

            ValidateIssuer = !string.IsNullOrWhiteSpace(jwtOpts.Issuer),
            ValidIssuer = jwtOpts.Issuer,

            ValidateAudience = !string.IsNullOrWhiteSpace(jwtOpts.Audience),
            ValidAudience = jwtOpts.Audience,

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero // reject expired tokens immediately
        };
    });

var app = builder.Build();

// ----------------------------
// Pipeline
// ----------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseCors("web");
// Global exception handler (your custom middleware)
app.UseExceptionHandlingMiddleware();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// A tiny test endpoint group (optional)
app.MapGet("/", () => Results.Ok(new { ok = true, service = "eCommerce API" }));

app.Run();



