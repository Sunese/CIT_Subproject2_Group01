using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Application.Context;

public class ImdbContext : DbContext
{
    private readonly string _connectionString;
    public DbSet<Title> Titles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<TitleBookmark> TitleBookmarks { get; set; }
    public DbSet<NameBookmark> NameBookmarks { get; set; }
    public DbSet<TitleRating> TitleRatings { get; set; }

    public ImdbContext(IOptions<ImdbContextOptions> options)
    {
        _connectionString = options.Value.ConnectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.LogTo(Console.Out.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        // Set up configurations
        optionsBuilder.UseNpgsql(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Title>()
            .ToTable("title");
        modelBuilder.Entity<Title>()
            .HasKey(t => t.TitleID);
        modelBuilder.Entity<Title>()
            .Property(t => t.TitleID)
            .HasColumnName("titleid");
        modelBuilder.Entity<Title>()
            .Property(t => t.PrimaryTitle)
            .HasColumnName("primarytitle");
        modelBuilder.Entity<Title>()
            .Property(t => t.OriginalTitle)
            .HasColumnName("originaltitle");
        modelBuilder.Entity<Title>()
            .Property(t => t.TitleType)
            .HasColumnName("titletype");
        modelBuilder.Entity<Title>()
            .Property(t => t.IsAdult)
            .HasColumnName("isadult");
        modelBuilder.Entity<Title>()
            .Property(t => t.Released)
            .HasColumnName("released");
        modelBuilder.Entity<Title>()
            .Property(t => t.RuntimeMinutes)
            .HasColumnName("runtimeminutes");
        modelBuilder.Entity<Title>().Property(t => t.Poster)
            .HasColumnName("poster");
        modelBuilder.Entity<Title>().Property(t => t.Plot)
            .HasColumnName("plot");
        modelBuilder.Entity<Title>().Property(t => t.StartYear)
            .HasColumnName("startyear")
            .HasConversion<YearConverter>();
        modelBuilder.Entity<Title>().Property(t => t.EndYear)
            .HasColumnName("endyear")
            .HasConversion<YearConverter>();
        modelBuilder.Entity<Title>()
            // Many-to-many relationship between Title and Genre
            // using TitleGenre as the join table
            // https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many#unidirectional-many-to-many
            .HasMany(t => t.Genres)
            .WithMany()
            .UsingEntity<TitleGenre>();
        modelBuilder.Entity<Title>()
            .HasOne(t => t.TitleRating)
            .WithMany()
            .HasForeignKey(t => t.TitleID);

    modelBuilder.Entity<User>()
            .ToTable("users");
        modelBuilder.Entity<User>()
            .HasKey(u => u.UserName);
        modelBuilder.Entity<User>()
            .Property(u => u.UserName)
            .HasColumnName("username");
        modelBuilder.Entity<User>()
            .Property(u => u.Password)
            .HasColumnName("password");
        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .HasColumnName("email");
        modelBuilder.Entity<User>()
            .Property(u => u.Salt)
            .HasColumnName("salt");
        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasColumnName("role");

        modelBuilder.Entity<TitleBookmark>()
            .ToTable("titlebookmark");
        modelBuilder.Entity<TitleBookmark>()
            .Property(tb => tb.Username)
            .HasColumnName("username");
        modelBuilder.Entity<TitleBookmark>()
            .Property(tb => tb.TitleId)
            .HasColumnName("titleid");
        modelBuilder.Entity<TitleBookmark>()
            .Property(tb => tb.Timestamp)
            .HasColumnName("timestamp");
        modelBuilder.Entity<TitleBookmark>()
            .Property(tb => tb.Notes)
            .HasColumnName("notes");
        modelBuilder.Entity<TitleBookmark>()
            .HasKey(tb => new { tb.Username, tb.TitleId });

        modelBuilder.Entity<NameBookmark>()
            .ToTable("namebookmark");
        modelBuilder.Entity<NameBookmark>()
            .Property(nb => nb.Username)
            .HasColumnName("username");
        modelBuilder.Entity<NameBookmark>()
            .Property(nb => nb.NameId)
            .HasColumnName("nameid");
        modelBuilder.Entity<NameBookmark>()
            .Property(nb => nb.Timestamp)
            .HasColumnName("timestamp");
        modelBuilder.Entity<NameBookmark>() 
            .Property(nb => nb.Notes)
            .HasColumnName("notes");
        modelBuilder.Entity<NameBookmark>()
            .HasKey(nb => new { nb.Username, nb.NameId });

        modelBuilder.Entity<Genre>()
            .ToTable("genre");
        modelBuilder.Entity<Genre>()
            .HasKey(g => g.GenreName);
        modelBuilder.Entity<Genre>()
            .Property(g => g.GenreName)
            .HasColumnName("genrename");

        modelBuilder.Entity<TitleGenre>()
            .ToTable("titlegenre");
        modelBuilder.Entity<TitleGenre>()
            .HasKey(tg => new { tg.TitleId, tg.GenreName });
        modelBuilder.Entity<TitleGenre>()
            .Property(tg => tg.TitleId)
            .HasColumnName("titleid");
        modelBuilder.Entity<TitleGenre>()
            .Property(tg => tg.GenreName)
            .HasColumnName("genrename");

        // TitleRating
        modelBuilder.Entity<TitleRating>()
            .ToTable("titlerating");
        modelBuilder.Entity<TitleRating>()
            .HasKey(tr => tr.TitleID);
        modelBuilder.Entity<TitleRating>()
            .Property(tr => tr.TitleID)
            .HasColumnName("titleid");
        modelBuilder.Entity<TitleRating>()
            .Property(tr => tr.AverageRating)
            .HasColumnName("averagerating");
        modelBuilder.Entity<TitleRating>()
            .Property(tr => tr.NumVotes)
            .HasColumnName("numvotes");
    }
}