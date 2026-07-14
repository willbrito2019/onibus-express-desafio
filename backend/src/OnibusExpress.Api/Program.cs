using Microsoft.EntityFrameworkCore;
using OnibusExpress.Application.Services;
using OnibusExpress.Domain.Interfaces;
using OnibusExpress.Infrastructure.Persistence;
using OnibusExpress.Infrastructure.Repositories;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IRotaRepository, RotaRepository>();
builder.Services.AddScoped<IViagemRepository, ViagemRepository>();
builder.Services.AddScoped<IPassageiroRepository, PassageiroRepository>();
builder.Services.AddScoped<IReservaRepository, ReservaRepository>();
builder.Services.AddScoped<IGeradorCodigoReserva, GeradorCodigoReserva>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    DbSeeder.Seed(db);
}

app.Run();
