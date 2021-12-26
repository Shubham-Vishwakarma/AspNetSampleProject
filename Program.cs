using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using NLog.Web;
using System;
using BuildRestApiNetCore.Models;
using BuildRestApiNetCore.Services.Auth;
using BuildRestApiNetCore.Services.Customers;
using BuildRestApiNetCore.Services.Products;
using BuildRestApiNetCore.Middleware;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
logger.Debug("Init Main");

try 
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    var appSettingsSection = builder.Configuration.GetSection("AppSettings");
    builder.Services.Configure<AppSettings>(appSettingsSection);

    builder.Services.AddScoped<ICustomerService, CustomerService>();
    builder.Services.AddScoped<IProductService, ProductService>();
    builder.Services.AddScoped<IAuthService, AuthService>();

    // Add Database Connection
    string connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
    
    builder.Services.AddDbContextPool<ShopbridgeContext>(
        options => {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), mySqlOptionsAction: mysqlOptions => { mysqlOptions.EnableRetryOnFailure(); });
        }
    );

    builder.Services.AddCors();
    builder.Services.AddControllers().AddNewtonsoftJson();

    builder.Services.AddApiVersioning(opt => {
        opt.ReportApiVersions = true;
        opt.AssumeDefaultVersionWhenUnspecified = true;
    });

    // Ignore Self Looping
    builder.Services.AddMvc(options => options.EnableEndpointRouting = false)
                .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo{
        Title = "ShopBridge",
        Description = "The ultimate e-commerce store for all your needs",
        Version = "v1"
    }));


    var key = Encoding.ASCII.GetBytes("TOP_SECRET_KEY_USED_FOR_SIGNING_AND_VERIYING_JWT_TOKENS");

    builder.Services.AddAuthentication(x =>{
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(opt => opt.SwaggerEndpoint("/swagger/v1/swagger.json", "ShopBridge v1"));
    }

    app.UseCors(x => x.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());

    app.UseMiddleware<JwtMiddleWare>();
    app.UseAuthentication();
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