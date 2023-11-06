using Application.Context;
using Application.Models;
using Application.Profiles;
using Application.Services;
using Application.UnitTests.Data;
using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;

namespace Application.UnitTests.Services;

public class BookmarkServiceTests
{
    // Naming convention: MethodName_StateUnderTest_ExpectedBehavior
    [Theory]
    [MemberData(nameof(TitleBookmarkServiceTestData.Data), MemberType = typeof(TitleBookmarkServiceTestData))]
    public void GetTitleBookmarks_ExistingUsername_ReturnsBookmarks(
        IMapper mapper,
        DbContextOptions<ImdbContext> dbContextOptions
    )
    {
        // Arrange
        using var context = new ImdbContext(dbContextOptions);
        var service = new BookmarkService(context, mapper);
        // Act
        var bookmarks = service.GetTitleBookmarks("testUser", OrderBy.Alphabetical, 10);
        // Assert
        Assert.Equal(2, bookmarks.Count);
        Assert.Equal("tt0000001", bookmarks[0].TitleId);
        Assert.Equal("tt0000002", bookmarks[1].TitleId);
    }

    [Theory]
    [MemberData(nameof(TitleBookmarkServiceTestData.Data), MemberType = typeof(TitleBookmarkServiceTestData))]
    public void GetTitleBookmarks_NonExistingUsername_ReturnsEmptyList(
        IMapper mapper,
        DbContextOptions<ImdbContext> dbContextOptions
    )
    {
        // Arrange
        using var context = new ImdbContext(dbContextOptions);
        var service = new BookmarkService(context, mapper);
        // Act
        var bookmarks = service.GetTitleBookmarks("nonExistingUser", OrderBy.Alphabetical, 10);
        // Assert
        Assert.Empty(bookmarks);
    }

    [Theory]
    [MemberData(nameof(NameBookmarkServiceTestData.Data), MemberType = typeof(NameBookmarkServiceTestData))]
    public void GetNameBookmarks_ExistingUsername_ReturnsBookmarks(
        IMapper mapper,
        DbContextOptions<ImdbContext> dbContextOptions
    )
    {
        // Arrange
        using var context = new ImdbContext(dbContextOptions);
        var service = new BookmarkService(context, mapper);
        // Act
        var bookmarks = service.GetNameBookmarks("testUser", OrderBy.Alphabetical, 10);
        // Assert
        Assert.Equal(2, bookmarks.Count);
        Assert.Equal("nm0000001", bookmarks[0].NameId);
        Assert.Equal("nm0000002", bookmarks[1].NameId);
    }

    [Theory]
    [MemberData(nameof(NameBookmarkServiceTestData.Data), MemberType = typeof(NameBookmarkServiceTestData))]
    public void GetNameBookmarks_NonExistingUsername_ReturnsEmptyList(
        IMapper mapper,
        DbContextOptions<ImdbContext> dbContextOptions
    )
    {
        // Arrange
        using var context = new ImdbContext(dbContextOptions);
        var service = new BookmarkService(context, mapper);
        // Act
        var bookmarks = service.GetNameBookmarks("nonExistingUser", OrderBy.Alphabetical, 10);
        // Assert
        Assert.Empty(bookmarks);
    }
}