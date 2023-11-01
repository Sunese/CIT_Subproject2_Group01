using Microsoft.Extensions.Options;

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

        var bookmarks = new List<TitleBookmark>
        {
            new()
            {
                Username = "testUser",
                TitleId = "tt0000001",
                Timestamp = DateTime.Now,
                Notes = "Test notes"
            },
            new()
            {
                Username = "testUser",
                TitleId = "tt0000002",
                Timestamp = DateTime.Now,
                Notes = "Test notes"
            }
        };

        var mockSet = new Mock<DbSet<TitleBookmark>>();
        // https://learn.microsoft.com/en-us/ef/ef6/fundamentals/testing/mocking#testing-query-scenarios
        mockSet.As<IQueryable<TitleBookmark>>()
            .Setup(m => m.Provider)
            .Returns(bookmarks.AsQueryable().Provider);
        mockSet.As<IQueryable<TitleBookmark>>()
            .Setup(m => m.Expression)
            .Returns(bookmarks.AsQueryable().Expression);
        mockSet.As<IQueryable<TitleBookmark>>()
            .Setup(m => m.ElementType)
            .Returns(bookmarks.AsQueryable().ElementType);
        mockSet.As<IQueryable<TitleBookmark>>()
            .Setup(m => m.GetEnumerator())
            .Returns(bookmarks.AsQueryable().GetEnumerator());

        // Using Options.Create() here is simpler than mocking an IOptions with moq
        var mockOptions = Options.Create(new ImdbContextOptions {ConnectionString = ""});
        var mockContext = new Mock<ImdbContext>(mockOptions);
        mockContext.Setup(x => x.TitleBookmarks).Returns(mockSet.Object);

        return new List<object[]>
        {
            new object[] { mapper, mockContext }
        };
    }
}
