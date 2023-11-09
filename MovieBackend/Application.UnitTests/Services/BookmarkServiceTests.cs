using Application.Context;
using Application.Models;
using Application.Profiles;
using Application.Services;
using Application.UnitTests.Fixture;
using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Application.UnitTests.Services;

[CollectionDefinition("DatabaseCollection")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
}

[Collection("DatabaseCollection")]
public class BookmarkServiceTests
{
    DatabaseFixture _fixture;
    private Mock<IMapper> _mockMapper;

    // In these tests we are abstracting away the dependencies the BookmarkService
    // has, which are the database context and the mapper.
    // The database context is abstracted away by using an
    // in-memory database, which is passsed from the DatabaseFixture class.
    // The mapper is abstracted away by using a mock mapper, which is
    // pre-configured to return an empty DTO object when mapping from a domain
    // object.
    // A relevant test in this context is to check whether the Map() function
    // was called, since this means that the service managed to complete the
    // Linq expression and is mapping the result to a DTO which will immediately
    // be returned.
    public BookmarkServiceTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(m =>
                m.Map<IList<TitleBookmarkDTO>>(It.IsAny<List<TitleBookmark>>()))
                .Returns(new List<TitleBookmarkDTO>());
        mockMapper.Setup(m =>
                m.Map<NameBookmarkDTO>(It.IsAny<List<NameBookmark>>()))
                .Returns(new NameBookmarkDTO());
        _mockMapper = mockMapper;
    }

    // Naming convention: MethodName_StateUnderTest_ExpectedBehavior
    [Fact]
    public void GetTitleBookmarks_ExistingUsername_ReturnsBookmarks()
    {
        // Arrange
        using (var context = new ImdbContext(_fixture.ContextOptions))
        {
            var service = new BookmarkService(context, _mockMapper.Object);
            // Act
            var bookmarks = service.GetTitleBookmarks("testUser", OrderBy.Alphabetical, 0, 10);
            // Assert
            _mockMapper.Verify(x => x.Map<IList<TitleBookmarkDTO>>(It.IsAny<List<TitleBookmark>>()), Times.Once());
        }
    }

    [Fact]
    public void GetTitleBookmarks_NonExistingUsername_MapperIsCalledOnce()
    {
        // Arrange
        using (var context = new ImdbContext(_fixture.ContextOptions))
        {
            var service = new BookmarkService(context, _mockMapper.Object);
            // Act
            var bookmarks = service.GetTitleBookmarks("nonExistingUser", OrderBy.Alphabetical, 0, 10);
            // Assert
            _mockMapper.Verify(x => x.Map<IList<TitleBookmarkDTO>>(It.IsAny<List<TitleBookmark>>()), Times.Once());
        }
    }

    [Fact]
    public void GetNameBookmarks_ExistingUsername_ReturnsBookmarks()
    {
        using (var context = new ImdbContext(_fixture.ContextOptions))
        {
            var service = new BookmarkService(context, _mockMapper.Object);
            // Act
            var bookmarks = service.GetNameBookmarks("testUser", OrderBy.Alphabetical, 0, 10);
            // Assert
            _mockMapper.Verify(x => x.Map<IList<NameBookmarkDTO>>(It.IsAny<List<NameBookmark>>()), Times.Once());
        }
    }

    [Fact]
    public void GetNameBookmarks_NonExistingUsername_ReturnsEmptyList()
    {
        // Arrange
        using (var context = new ImdbContext(_fixture.ContextOptions))
        {
            var service = new BookmarkService(context, _mockMapper.Object);
            // Act
            var bookmarks = service.GetNameBookmarks("nonExistingUser", OrderBy.Alphabetical, 0, 10);
            // Assert
            _mockMapper.Verify(x => x.Map<IList<NameBookmarkDTO>>(It.IsAny<List<NameBookmark>>()), Times.Once());
        }
    }
}