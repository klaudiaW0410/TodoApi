using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TodoApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoSettings"));

// Register ToDoService
builder.Services.AddSingleton<ToDoService>();

// Add CORS policy
builder.Services.AddCors(options =>
{
  options.AddDefaultPolicy(builder =>
  {
    builder.WithOrigins("http://localhost:5175") // Adres frontendowy
             .AllowAnyHeader()
             .AllowAnyMethod();
  });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Ensure CORS is used
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
