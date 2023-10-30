using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Context;
using Application.Models;

namespace Application.Services;

public class BookmarkService : IBookmarkService
{
    private readonly ImdbContext _context;
    public BookmarkService(ImdbContext context)
    {
        _context = context;
    }

    public void CreateNameBookmark(string username, NameBookmarkDTO model)
    {
        throw new NotImplementedException();
    }

    public void CreateTitleBookmark(string username, TitleBookmarkDTO model)
    {
        throw new NotImplementedException();
    }

    public void DeleteNameBookmark(string username, NameBookmarkDTO model)
    {
        throw new NotImplementedException();
    }

    public void DeleteTitleBookmark(string username, TitleBookmarkDTO model)
    {
        throw new NotImplementedException();
    }

    public void UpdateNameBookmark(string username, NameBookmarkDTO model)
    {
        throw new NotImplementedException();
    }

    public void UpdateTitleBookmark(string username, TitleBookmarkDTO model)
    {
        throw new NotImplementedException();
    }
}