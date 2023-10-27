using Application.Context;
using Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Configuration.AddJsonFile("config.json", false);

builder.Services.Configure<ImdbContextOptions>(
    builder.Configuration.GetSection(ImdbContextOptions.ImdbContext));

builder.Services.AddControllers();

builder.Services.AddSingleton<ImdbContext>();
builder.Services.AddScoped<IImdbService, ImdbService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

