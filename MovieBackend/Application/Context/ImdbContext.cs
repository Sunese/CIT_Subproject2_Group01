using Application.Models;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Type = Domain.Models.Type;

namespace Application.Context;

public class ImdbContext : DbContext
{

    public DbSet<Title> Titles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Name> Names { get; set; }
    public DbSet<Search> Searches { get; set; }
    public DbSet<TitleBookmark> TitleBookmarks { get; set; }
    public DbSet<NameBookmark> NameBookmarks { get; set; }
    public DbSet<TitleRating> TitleRatings { get; set; }
    public DbSet<UserTitleRating> UserTitleRatings { get; set; }
    public DbSet<TitleSearchResult> TitleSearchResults { get; set; }
    public DbSet<NameSearchResult> NameSearchResults { get; set; }
    public DbSet<CoPlayers> CoPlayers { get; set; }
    public DbSet<PopularActorsResult> PopularActorsResults { get; set; }
    public DbSet<NameRating> NameRatings { get; set; }
    public DbSet<Aka> Aka { get; set; }
    public DbSet<AkaType> AkaType { get; set; }
    public DbSet<Type> Type { get; set; }
    public DbSet<Episode> Episodes { get; set; }
    public DbSet<Writer> Writers { get; set; }
    public DbSet<Director> Directors { get; set; }
    public DbSet<SimiliarMoviesResult> SimiliarMoviesResult { get; set; }

    public ImdbContext(
        DbContextOptions<ImdbContext> dbContextOptions) : base(dbContextOptions)
    {
    }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.EnableSensitiveDataLogging();
    //     optionsBuilder.LogTo(Console.Out.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
    //     // Set up configurations
    //     optionsBuilder.UseNpgsql(_connectionString);
    // }

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
            // Many-to-many relationship between TitleName and Genre
            // using TitleGenre as the join table
            // https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many#unidirectional-many-to-many
            .HasMany(t => t.Genres)
            .WithMany()
            .UsingEntity<TitleGenre>();
        modelBuilder.Entity<Title>()
            .HasOne(t => t.TitleRating)
            .WithMany()
            .HasForeignKey(t => t.TitleID)
            .IsRequired(false);
        modelBuilder.Entity<Title>()
            .HasMany(t => t.Akas)
            .WithOne(a => a.Title)
            .HasForeignKey(t =>  t.TitleId)
            .IsRequired();

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

        // Name
        modelBuilder.Entity<Name>()
            .ToTable("name");
        modelBuilder.Entity<Name>()
            .HasKey(n => n.NameId);
        modelBuilder.Entity<Name>()
            .Property(n => n.NameId)
            .HasColumnName("nameid");
        modelBuilder.Entity<Name>()
            .Property(n => n.PrimaryName)
            .HasColumnName("primaryname");
        modelBuilder.Entity<Name>()
            .Property(n => n.BirthYear)
            .HasColumnName("birthyear");
        modelBuilder.Entity<Name>()
            .Property(n => n.DeathYear)
            .HasColumnName("deathyear");
        modelBuilder.Entity<Name>()
            .HasMany(n => n.PrimaryProfessions)
            .WithMany()
            .UsingEntity<PrimaryProfession>();
        modelBuilder.Entity<Name>()
            .HasMany(n => n.KnownForTitles)
            .WithMany()
            .UsingEntity<KnownForTitles>();
        // modelBuilder.Entity<Name>()
        //     .HasMany(n => n.Principals)
        //     .WithOne(p => p.Name)
        //     .HasForeignKey(p => p.NameID)
        //     .IsRequired(false);

        // Principals
        modelBuilder.Entity<Principal>()
            .ToTable("principals");
        modelBuilder.Entity<Principal>()
            .HasKey(p => new { p.TitleID, p.Ordering });
        modelBuilder.Entity<Principal>()
            .Property(p => p.TitleID)
            .HasColumnName("titleid");
        modelBuilder.Entity<Principal>()
            .Property(p => p.NameID)
            .HasColumnName("nameid");
        modelBuilder.Entity<Principal>()
            .Property(p => p.Ordering)
            .HasColumnName("ordering");
        modelBuilder.Entity<Principal>()
            .Property(p => p.Category)
            .HasColumnName("category");
        modelBuilder.Entity<Principal>()
            .Property(p => p.Job)
            .HasColumnName("job");
        modelBuilder.Entity<Principal>()
            .HasMany(p => p.Characters)
            .WithOne(c => c.Principal)
            .HasForeignKey(c => new { c.TitleID, c.Ordering })
            .IsRequired();

        // Character
        modelBuilder.Entity<Character>()
            .ToTable("characters");
        modelBuilder.Entity<Character>()
            .HasKey(c => new { c.TitleID, c.CharacterName, c.Ordering });
        modelBuilder.Entity<Character>()
            .Property(c => c.TitleID)
            .HasColumnName("titleid");
        modelBuilder.Entity<Character>()
            .Property(c => c.Ordering)
            .HasColumnName("ordering");
        modelBuilder.Entity<Character>()
            .Property(c => c.CharacterName)
            .HasColumnName("charactername");

        // KnownForTitles
        modelBuilder.Entity<KnownForTitles>()
            .ToTable("knownfortitles");
        modelBuilder.Entity<KnownForTitles>()
            .HasKey(kft => new { kft.NameID, kft.TitleID });
        modelBuilder.Entity<KnownForTitles>()
            .Property(kft => kft.NameID)
            .HasColumnName("nameid");
        modelBuilder.Entity<KnownForTitles>()
            .Property(kft => kft.TitleID)
            .HasColumnName("titleid");

        // Profession
        modelBuilder.Entity<Profession>()
            .ToTable("professions");
        modelBuilder.Entity<Profession>()
            .HasKey(p => p.ProfessionName);
        modelBuilder.Entity<Profession>()
            .Property(p => p.ProfessionName)
            .HasColumnName("professionname");

        // PrimaryProfession
        modelBuilder.Entity<PrimaryProfession>()
            .ToTable("primaryprofession");
        modelBuilder.Entity<PrimaryProfession>()
            .HasKey(pp => new { pp.NameID, pp.ProfessionName });
        modelBuilder.Entity<PrimaryProfession>()
            .Property(pp => pp.NameID)
            .HasColumnName("nameid");
        modelBuilder.Entity<PrimaryProfession>()
            .Property(pp => pp.ProfessionName)
            .HasColumnName("professionname");

        // Search
        modelBuilder.Entity<Search>()
            .ToTable("search");
        modelBuilder.Entity<Search>()
            .HasKey(s => new { s.Username, s.Timestamp });
        modelBuilder.Entity<Search>()
            .Property(s => s.Username)
            .HasColumnName("username");
        modelBuilder.Entity<Search>()
            .Property(s => s.Query)
            .HasColumnName("query");
        modelBuilder.Entity<Search>()
            .Property(s => s.Timestamp)
            .HasColumnName("timestamp");

        // UserTitleRating
        modelBuilder.Entity<UserTitleRating>()
            .ToTable("userrating");
        modelBuilder.Entity<UserTitleRating>()
            .HasKey(ur => new { ur.Username, ur.TitleId });
        modelBuilder.Entity<UserTitleRating>()
            .Property(n => n.Username)
            .HasColumnName("username");
        modelBuilder.Entity<UserTitleRating>()
            .Property(n => n.TitleId)
            .HasColumnName("titleid");
        modelBuilder.Entity<UserTitleRating>()
            .Property(n => n.Rating)
            .HasColumnName("rating");
        modelBuilder.Entity<UserTitleRating>()
            .Property(n => n.TimeStamp)
            .HasColumnName("timestamp");

        // TitleSearchResult
        modelBuilder.Entity<TitleSearchResult>()
            .HasNoKey();
        modelBuilder.Entity<TitleSearchResult>()
            .Property(tsr => tsr.TitleID)
            .HasColumnName("titleid");
        modelBuilder.Entity<TitleSearchResult>()
            .Property(tsr => tsr.PrimaryTitle)
            .HasColumnName("primarytitle");
        modelBuilder.Entity<TitleSearchResult>()
            .Property(tsr => tsr.Rank)
            .HasColumnName("rank");

        // NameRating
        modelBuilder.Entity<NameRating>()
            .ToTable("namerating");
        modelBuilder.Entity<NameRating>()
            .HasKey(nr => nr.NameId);
        modelBuilder.Entity<NameRating>()
            .Property(nr => nr.NameId)
            .HasColumnName("nameid");
        modelBuilder.Entity<NameRating>()
            .Property(nr => nr.Rating)
            .HasColumnName("rating");


        // Popular Actors result
        modelBuilder.Entity<PopularActorsResult>()
            .HasNoKey();
        modelBuilder.Entity<PopularActorsResult>()
            .Property(par => par.NameId)
            .HasColumnName("nameid");
        modelBuilder.Entity<PopularActorsResult>()
            .Property(par => par.PrimaryName)
            .HasColumnName("primaryname");
        modelBuilder.Entity<PopularActorsResult>()
            .Property(par => par.Rating)
            .HasColumnName("rating");

        // NameSearchResult
        modelBuilder.Entity<NameSearchResult>()
            .HasNoKey();
        modelBuilder.Entity<NameSearchResult>()
            .Property(nsr => nsr.NameId)
            .HasColumnName("nameid");
        modelBuilder.Entity<NameSearchResult>()
            .Property(nsr => nsr.PrimaryName)
            .HasColumnName("primaryname");

        // CoPlayers
        modelBuilder.Entity<CoPlayers>()
            .HasNoKey();
        modelBuilder.Entity<CoPlayers>()
            .Property(cp => cp.NameId)
            .HasColumnName("nameid");
        modelBuilder.Entity<CoPlayers>()
            .Property(cp => cp.PrimaryName)
            .HasColumnName("primaryname");
        modelBuilder.Entity<CoPlayers>()
            .Property(cp => cp.Frequency)
            .HasColumnName("frequency");

        // TitleAkas
        modelBuilder.Entity<Aka>()
            .ToTable("akas"); 
        modelBuilder.Entity<Aka>()
            .HasKey(a => new { a.TitleId, a.Ordering });
        modelBuilder.Entity<Aka>()
            .Property(a => a.TitleId)
            .HasColumnName("titleid");
        modelBuilder.Entity<Aka>()
            .Property(a => a.Ordering)
            .HasColumnName("ordering");
        modelBuilder.Entity<Aka>()
            .Property(a => a.TitleName)
            .HasColumnName("title");
        modelBuilder.Entity<Aka>()
            .Property(a => a.Region)
            .HasColumnName("region");
        modelBuilder.Entity<Aka>()
            .Property(a => a.Attribtues)
            .HasColumnName("attributes");
        modelBuilder.Entity<Aka>()
            .Property(a => a.Language)
            .HasColumnName("language");
        modelBuilder.Entity<Aka>()
            .Property(a => a.IsOriginalTitle)
            .HasColumnName("isoriginaltitle");
        modelBuilder.Entity<Aka>()
            .HasMany(t => t.Types)
            .WithMany()
            .UsingEntity<AkaType>();


        // AkaType
        modelBuilder.Entity<AkaType>()
            .ToTable("akastypes");
        modelBuilder.Entity<AkaType>()
            .HasKey(at => new { at.TitleId, at.Ordering, at.TypeName });
        modelBuilder.Entity<AkaType>()
            .Property(at => at.TitleId)
            .HasColumnName("titleid");
        modelBuilder.Entity<AkaType>()
            .Property(at => at.Ordering)
            .HasColumnName("ordering");
        modelBuilder.Entity<AkaType>()
            .Property(at => at.TypeName)
            .HasColumnName("typename");

        // Type
        modelBuilder.Entity<Type>()
            .ToTable("types");
        modelBuilder.Entity<Type>()
            .HasKey(t => t.TypeName);
        modelBuilder.Entity<Type>()
            .Property(at => at.TypeName)
            .HasColumnName("typename");

        // Director
        modelBuilder.Entity<Director>()
            .ToTable("directors");
        modelBuilder.Entity<Director>()
            .HasKey(d => new { d.TitleID, d.NameId });
        modelBuilder.Entity<Director>()
            .Property(d => d.TitleID)
            .HasColumnName("titleid");
        modelBuilder.Entity<Director>()
            .Property(d => d.NameId)
            .HasColumnName("nameid");

        // Writer
        modelBuilder.Entity<Writer>()
            .ToTable("writers");
        modelBuilder.Entity<Writer>()
            .HasKey(w => new { w.TitleID, w.NameId });
        modelBuilder.Entity<Writer>()
            .Property(w => w.TitleID)
            .HasColumnName("titleid");
        modelBuilder.Entity<Writer>()
            .Property(w => w.NameId)
            .HasColumnName("nameid");

        // Episode
        modelBuilder.Entity<Episode>()
            .ToTable("episode");
        modelBuilder.Entity<Episode>()
            .HasKey(e => new { e.TitleID, e.ParentTitleID });
        modelBuilder.Entity<Episode>()
            .Property(e => e.TitleID)
            .HasColumnName("titleid");
        modelBuilder.Entity<Episode>()
            .Property(e => e.ParentTitleID)
            .HasColumnName("parenttitleid");
        modelBuilder.Entity<Episode>()
            .Property(e => e.EpisodeNumber)
            .HasColumnName("episodenumber");
        modelBuilder.Entity<Episode>()
            .Property(e => e.SeasonNumber)
            .HasColumnName("seasonnumber");

        // PopularActorsResult
        modelBuilder.Entity<PopularActorsResult>()
            .HasNoKey();
        modelBuilder.Entity<PopularActorsResult>()
            .Property(par => par.NameId)
            .HasColumnName("nameid");
        modelBuilder.Entity<PopularActorsResult>()
            .Property(par => par.PrimaryName)
            .HasColumnName("primaryname");
        modelBuilder.Entity<PopularActorsResult>()
            .Property(par => par.Rating)
            .HasColumnName("rating");

        // SimiliarMovieResult
        modelBuilder.Entity<SimiliarMoviesResult>()
            .HasNoKey();
        modelBuilder.Entity<SimiliarMoviesResult>()
            .Property(smr => smr.TitleId)
            .HasColumnName("_titleid");
        modelBuilder.Entity<SimiliarMoviesResult>()
            .Property(smr => smr.PrimaryTitle)
            .HasColumnName("primarytitle");
        modelBuilder.Entity<SimiliarMoviesResult>()
            .Property(smr => smr.StartYear)
            .HasColumnName("startyear");
        modelBuilder.Entity<SimiliarMoviesResult>()
            .Property(smr => smr.RunTimeMinutes)
            .HasColumnName("runtimeminutes");
    }
}