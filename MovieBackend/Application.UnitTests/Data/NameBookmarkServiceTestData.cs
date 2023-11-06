using Application.Context;
using Application.Profiles;
using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;

namespace Application.UnitTests.Data;

public class NameBookmarkServiceTestData
{
    public static IEnumerable<object[]> Data()
    {
        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new BookmarkProfile());
        });
        var mapper = mockMapper.CreateMapper();

        // Using Options.Create() here is simpler than mocking an IOptions with moq
        // var mockOptions = Options.Create(new ImdbContextOptions {ConnectionString = ""});

        var dbContextOptions = new DbContextOptionsBuilder<ImdbContext>()
            .UseInMemoryDatabase(databaseName: "ImdbTestDatabase")
            .Options;

        // Insert seed data into the database using one instance of the context
        using (var context = new ImdbContext(dbContextOptions))
        {
            context.Names.Add(new Name{
                NameId = "nm0000001",
                PrimaryName = "TestName1",
                BirthYear = "2000",
                DeathYear = "2025"
            });
            context.Names.Add(new Name{
                NameId = "nm0000002",
                PrimaryName = "TestName2",
                BirthYear = "2000",
                DeathYear = "2025"
            });
            context.Add(new User { UserName = "testUser", Password = "x", Email = "x", Role = "User", Salt = "x" });
            context.NameBookmarks.Add(new NameBookmark { Username = "testUser", NameId = "nm0000001", Timestamp = DateTime.Now });
            context.NameBookmarks.Add(new NameBookmark { Username = "testUser", NameId = "nm0000002", Timestamp = DateTime.Now });
            context.SaveChanges();
        }

        return new List<object[]>
        {
            new object[] { mapper, dbContextOptions }
        };
    }
}
