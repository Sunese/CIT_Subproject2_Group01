using Microsoft.Extensions.Options;

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

        var bookmarks = new List<NameBookmark>
        {
            new()
            {
                Username = "testUser",
                NameId = "nm0000001",
                Timestamp = DateTime.Now,
                Notes = "Test notes"
            },
            new()
            {
                Username = "testUser",
                NameId = "nm0000002",
                Timestamp = DateTime.Now,
                Notes = "Test notes"
            }
        };

        var mockSet = new Mock<DbSet<NameBookmark>>();
        // https://learn.microsoft.com/en-us/ef/ef6/fundamentals/testing/mocking#testing-query-scenarios
        mockSet.As<IQueryable<NameBookmark>>()
            .Setup(m => m.Provider)
            .Returns(bookmarks.AsQueryable().Provider);
        mockSet.As<IQueryable<NameBookmark>>()
            .Setup(m => m.Expression)
            .Returns(bookmarks.AsQueryable().Expression);
        mockSet.As<IQueryable<NameBookmark>>()
            .Setup(m => m.ElementType)
            .Returns(bookmarks.AsQueryable().ElementType);
        mockSet.As<IQueryable<NameBookmark>>()
            .Setup(m => m.GetEnumerator())
            .Returns(bookmarks.AsQueryable().GetEnumerator());

        // Using Options.Create() here is simpler than mocking an IOptions with moq
        var mockOptions = Options.Create(new ImdbContextOptions {ConnectionString = ""});
        var mockContext = new Mock<ImdbContext>(mockOptions);
        mockContext.Setup(x => x.NameBookmarks).Returns(mockSet.Object);

        return new List<object[]>
        {
            new object[] { mapper, mockContext }
        };
    }
}
