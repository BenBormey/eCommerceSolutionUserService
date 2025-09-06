using eCommerce.Infrastructure;
using eCommerce.Core;
using eCommerce.API.Middlewares;
using eCommerce.Core.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System.Text;
using AutoMapper;
using eCommerce.Core.Mappers;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Layer registrations
builder.Services.AddCore(builder.Configuration);
builder.Services.AddInfrastruction(builder.Configuration);

// AutoMapper profile(s)
builder.Services.AddAutoMapper(typeof(ApplicationUserMappingProfile).Assembly);

// JWT auth using JwtOptions
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
var jwtOpts = builder.Configuration.GetSection("Jwt").Get<JwtOptions>() ?? new JwtOptions();
var keyBytes = Encoding.UTF8.GetBytes(jwtOpts.SecretKey ?? string.Empty);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = !string.IsNullOrWhiteSpace(jwtOpts.Issuer),
        ValidateAudience = !string.IsNullOrWhiteSpace(jwtOpts.Audience),
        ValidIssuer = jwtOpts.Issuer,
        ValidAudience = jwtOpts.Audience
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandlingMiddleware();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
