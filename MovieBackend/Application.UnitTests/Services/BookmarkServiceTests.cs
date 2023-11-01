using Application.Context;
using Application.Models;
using Application.Profiles;
using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Application.UnitTests.Services;

public class BookmarkServiceTests
{
    // Naming convention: MethodName_StateUnderTest_ExpectedBehavior
    [Theory]
    [MemberData(nameof(TitleBookmarkServiceTestData.Data), MemberType = typeof(TitleBookmarkServiceTestData))]
    public void GetTitleBookmarks_ExistingUsername_ReturnsBookmarks(
        IMapper mapper,
        Mock<ImdbContext> mockContext
    )
    {
        // Arrange
        var service = new BookmarkService(mockContext.Object, mapper);
        // Act
        var bookmarks = service.GetTitleBookmarks("testUser");
        // Assert
        Assert.Equal(2, bookmarks.Count);
        Assert.Equal("tt0000001", bookmarks[0].TitleId);
        Assert.Equal("tt0000002", bookmarks[1].TitleId);
    }

    [Theory]
    [MemberData(nameof(TitleBookmarkServiceTestData.Data), MemberType = typeof(TitleBookmarkServiceTestData))]
    public void GetTitleBookmarks_NonExistingUsername_ReturnsEmptyList(
        IMapper mapper,
        Mock<ImdbContext> mockContext
    )
    {
        // Arrange
        var service = new BookmarkService(mockContext.Object, mapper);
        // Act
        var bookmarks = service.GetTitleBookmarks("nonExistingUser");
        // Assert
        Assert.Empty(bookmarks);
    }

    [Theory]
    [MemberData(nameof(NameBookmarkServiceTestData.Data), MemberType = typeof(NameBookmarkServiceTestData))]
    public void GetNameBookmarks_ExistingUsername_ReturnsBookmarks(
        IMapper mapper,
        Mock<ImdbContext> mockContext
    )
    {
        // Arrange
        var service = new BookmarkService(mockContext.Object, mapper);
        // Act
        var bookmarks = service.GetNameBookmarks("testUser");
        // Assert
        Assert.Equal(2, bookmarks.Count);
        Assert.Equal("nm0000001", bookmarks[0].NameId);
        Assert.Equal("nm0000002", bookmarks[1].NameId);
    }

    [Theory]
    [MemberData(nameof(NameBookmarkServiceTestData.Data), MemberType = typeof(NameBookmarkServiceTestData))]
    public void GetNameBookmarks_NonExistingUsername_ReturnsEmptyList(
        IMapper mapper,
        Mock<ImdbContext> mockContext
    )
    {
        // Arrange
        var service = new BookmarkService(mockContext.Object, mapper);
        // Act
        var bookmarks = service.GetNameBookmarks("nonExistingUser");
        // Assert
        Assert.Empty(bookmarks);
    }
}