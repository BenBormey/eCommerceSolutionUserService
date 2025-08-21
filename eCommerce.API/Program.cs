using eCommerce.Infrastructure;
using AutoMapper;
using eCommerce.Core;


using eCommerce.API.Middlewares;
using eCommerce.Core.Mappers;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddInfrastruction();
builder.Services.AddCore();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(ApplicationUserMappingProfile).Assembly);
var app = builder.Build();

// Configure the HTTP request pipeline.
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
