using Application.Context;
using Application.Profiles;
using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;

namespace Application.UnitTests.Data;

public class TitleBookmarkServiceTestData
{
    public static IEnumerable<object[]> Data()
    {
        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new BookmarkProfile());
        });
        var mapper = mockMapper.CreateMapper();

        var dbContextOptions = new DbContextOptionsBuilder<ImdbContext>()
            .UseInMemoryDatabase(databaseName: "ImdbTestDatabase")
            .Options;

        using var context = new ImdbContext(dbContextOptions);
        {
            context.Titles.Add(new Title {
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
            context.Titles.Add(new Title {
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
            context.Users.Add(new User { UserName = "testUser", Password = "x", Email = "x", Role = "User", Salt = "x" });
            context.TitleBookmarks.Add(new TitleBookmark { Username = "testUser", TitleId = "tt0000001", Timestamp = DateTime.Now });
            context.TitleBookmarks.Add(new TitleBookmark { Username = "testUser", TitleId = "tt0000002", Timestamp = DateTime.Now });
            context.SaveChanges();
        }

        return new List<object[]>
        {
            new object[] { mapper, dbContextOptions }
        };
    }
}
