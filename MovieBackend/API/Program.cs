using Application.Context;
using Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using API.Security;

// https://www.npgsql.org/doc/types/datetime.html#timestamps-and-timezones
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

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
builder.Services.AddScoped<ITitleService, TitleService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IBookmarkService, BookmarkService>();
builder.Services.AddScoped<IUserRatingService, UserRatingService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

