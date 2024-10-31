using Trackr.API.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.ConfigureLogger();
builder.Services.ConfigureVersioning();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseGlobalExceptionHandler();
app.UseAuthorization();
app.UseApiVersioning();

app.MapControllers();

app.Run();
