using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NLog.Web;
using System;
using BuildRestApiNetCore.Models;

var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
logger.Debug("Init Main");

try 
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    string connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
    builder.Services.AddDbContextPool<LibraryContext>(
        options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
    );

    builder.Services.AddControllers().AddNewtonsoftJson();

    builder.Services.AddApiVersioning(opt => {
        opt.ReportApiVersions = true;
        opt.AssumeDefaultVersionWhenUnspecified = true;
    });

    
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo{
        Title = "Products",
        Description = "The ultimate e-commerce store for all your needs",
        Version = "v1"
    }));

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(opt => opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Products v1"));
    }

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch(Exception exception) 
{
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}