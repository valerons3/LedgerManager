using System.Reflection;
using FluentValidation.AspNetCore;
using LedgerManager.API.Middleware;
using LedgerManager.Application.Interfaces.Repositories;
using LedgerManager.Application.Interfaces.Services;
using LedgerManager.Application.Services;
using LedgerManager.Application.Validators;
using LedgerManager.Infrastructure.Services;
using LedgerManager.Persistence.DbContexts;
using LedgerManager.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

// Controllers and Validators
builder.Services.AddControllers().AddFluentValidation(c =>
{
    c.RegisterValidatorsFromAssemblyContaining<CreateAccountRequestValidator>();
});

// Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "LedgerManager API",
        Version = "v1",
        Description = "API для работы с ЛС и проживающими"
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});


// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IResidentRepository, ResidentRepository>();

// Services
builder.Services.AddScoped<IAccountNumberGenerator, AccountNumberGenerator>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IResidentService, ResidentService>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();