using Trackr.API.Infrastructure.Extensions;
using Trackr.Application.Extensions;
using Trackr.Infrastructure.Extensions;
using FluentValidation;
using System.Reflection;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureLogger();
builder.Services.ConfigureVersioning();
builder.Services.ConfigureAuthentication();

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();


builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseGlobalExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.UseApiVersioning();

app.UseCors(options =>
    options.WithOrigins("https://localhost:3000")
           .AllowCredentials()
           .AllowAnyHeader()
           .AllowAnyMethod());


app.MapControllers();

app.Run();
