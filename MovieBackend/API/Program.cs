using Application.Context;
using Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using API.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Configuration.AddJsonFile("config.json", false);

builder.Services.Configure<ImdbContextOptions>(
    builder.Configuration.GetSection(ImdbContextOptions.ImdbContext));
builder.Services.Configure<JwtAuthOptions>(
    builder.Configuration.GetSection(JwtAuthOptions.JwtAuth));

builder.Services.AddSingleton<IHashingService, HashingService>();
builder.Services.AddSingleton<IJwtProvider, JwtProvider>();

builder.Services.ConfigureOptions<ConfigureJwtBearerOptions>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();

builder.Services.AddControllers();

builder.Services.AddDbContext<ImdbContext>(); // Defaults to scoped
builder.Services.AddScoped<IImdbService, ImdbService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

