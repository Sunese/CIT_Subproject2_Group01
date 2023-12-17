using Application.Context;
using Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using API.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Npgsql;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

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

builder.Services.AddScoped<IAuthorizationHandler, UserExistsRequirementHandler>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserExists", policy => {
        policy.RequireAuthenticatedUser();
        policy.Requirements.Add(new UserExistsRequirement());
    });
    options.DefaultPolicy = options.GetPolicy("UserExists");
});

builder.Services.AddControllers();

builder.Services.AddDbContext<ImdbContext>(
    options =>
    {
        options.EnableSensitiveDataLogging();
        options.LogTo(Console.Out.WriteLine, LogLevel.Information);
        var connString = builder.Configuration.GetSection(ImdbContextOptions.ImdbContext)
            .GetValue<string>("ConnectionString");
        options.UseNpgsql(connString);
    }
); // Defaults to scoped
builder.Services.AddScoped<ITitleService, TitleService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IBookmarkService, BookmarkService>();
builder.Services.AddScoped<IUserRatingService, UserRatingService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<INameService, NameService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(
    options => options.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader()
  );

app.MapControllers();

app.Run();

public partial class Program { }
