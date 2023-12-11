using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UnitTests.Fixture;

public class DatabaseFixture : IDisposable
{
    public DbContextOptions<ImdbContext> ContextOptions { get; set; }
    public DatabaseFixture()
    {
        ContextOptions = new DbContextOptionsBuilder<ImdbContext>()
            .UseInMemoryDatabase(databaseName: "ImdbTestDatabase")
            .Options;
        SeedData();
    }

    public void Dispose()
    {
        WipeData();
    }

    private void SeedData()
    {
        using var context = new ImdbContext(ContextOptions);
        {
            context.Titles.Add(new Title
            {
                TitleID = "tt0000001",
                PrimaryTitle = "TestTitle1",
                OriginalTitle = "TestTitle1",
                TitleType = "movie",
                IsAdult = false,
                Released = new DateOnly(2021, 1, 1),
                RuntimeMinutes = 100,
                Poster = "https://www.imdb.com/title/tt0000001/mediaviewer/rm123456789",
                Plot = "TestPlot1",
                StartYear = 2021,
                EndYear = null,
                Genres = new List<Genre> { new Genre { GenreName = "Action" } },
                TitleRating = new TitleRating { TitleID = "tt0000001", AverageRating = 5.0, NumVotes = 1 }
            });
            context.SaveChanges();
            context.Titles.Add(new Title
            {
                TitleID = "tt0000002",
                PrimaryTitle = "TestTitle2",
                OriginalTitle = "TestTitle2",
                TitleType = "movie",
                IsAdult = false,
                Released = new DateOnly(2021, 1, 1),
                RuntimeMinutes = 100,
                Poster = "https://www.imdb.com/title/tt0000002/mediaviewer/rm123456789",
                Plot = "TestPlot2",
                StartYear = 2021,
                EndYear = null,
                Genres = new List<Genre> { new Genre { GenreName = "Drama" } },
                TitleRating = new TitleRating { TitleID = "tt0000002", AverageRating = 5.0, NumVotes = 1 }
            });
            context.SaveChanges();
            context.Names.Add(new Name
            {
                NameID = "nm0000001",
                PrimaryName = "TestName1",
                BirthYear = "2000",
                DeathYear = "2025"
            });
            context.SaveChanges();
            context.Names.Add(new Name
            {
                NameID = "nm0000002",
                PrimaryName = "TestName2",
                BirthYear = "2000",
                DeathYear = "2025"
            });
            context.SaveChanges();
            context.Users.Add(
                new User { UserName = "testUser", Password = "x", Email = "x", Role = "User", Salt = "x" });
            context.SaveChanges();
            context.TitleBookmarks.Add(new TitleBookmark
                { Username = "testUser", TitleID = "tt0000001", Timestamp = DateTime.Now });
            context.SaveChanges();
            context.TitleBookmarks.Add(new TitleBookmark
                { Username = "testUser", TitleID = "tt0000002", Timestamp = DateTime.Now });
            context.SaveChanges();
            context.NameBookmarks.Add(new NameBookmark
                { Username = "testUser", NameID = "nm0000001", Timestamp = DateTime.Now });
            context.SaveChanges();
            context.NameBookmarks.Add(new NameBookmark
                { Username = "testUser", NameID = "nm0000002", Timestamp = DateTime.Now });
            context.SaveChanges();
        }
    }

    private void WipeData()
    {
        using var context = new ImdbContext(ContextOptions);
        {
            context.Database.EnsureDeleted();
        }
    }
}