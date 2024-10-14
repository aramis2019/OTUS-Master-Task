using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OTUS.Wexford.WebHost.Models;
using OTUS.Wexford.Core.Abstractions.Repositories;
using OTUS.Wexford.Core.Domain.Administration;
using OTUS.Wexford.DataAccess.Data;
using OTUS.Wexford.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);



builder.Configuration.Sources.Clear();

builder.Configuration
    .AddJsonFile("Configs/appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"Configs/db.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables("OTUS_");


builder.Services.Configure<DbConfig>(builder.Configuration.GetSection("db2"));
//builder.Services.AddOptions<RoundTheCodeSync>().Bind(Configuration.GetSection("RoundTheCodeSync")).ValidateDataAnnotations();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Example of", Version = "v1" });

    // Generate XML docs
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddSingleton(typeof(IRepository<Employee>), (x) => new InMemoryRepository<Employee>(FakeDataFactory.Employees));
builder.Services.AddSingleton(typeof(IRepository<Role>), (x) => new InMemoryRepository<Role>(FakeDataFactory.Roles));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
