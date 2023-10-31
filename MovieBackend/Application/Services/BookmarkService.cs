using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Context;
using Application.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class BookmarkService : IBookmarkService
{
    private readonly ImdbContext _context;
    private readonly IMapper _mapper;
    public BookmarkService(
        ImdbContext context,
        IMapper automapper)
    {
        _context = context;
        _mapper = automapper;
    }

    public void CreateTitleBookmark(string username, TitleBookmarkDTO model)
    {
        FormattableString query = $"CALL AddTitleBookmark({username}, {model.TitleId}, {model.Notes})";
        _context.Database.ExecuteSqlInterpolated(query);
    }

    public IList<TitleBookmarkDTO> GetTitleBookmarks(string username)
    {
        var bookmarks = _context.TitleBookmarks
            .Where(b => b.Username == username)
            .ToList();
        return _mapper.Map<IList<TitleBookmarkDTO>>(bookmarks);
    }

    public void UpdateTitleBookmarkNote(string username, TitleBookmarkDTO model)
    {
        FormattableString query = $"CALL UpdateNoteTitleBookmark({username}, {model.TitleId}, {model.Notes})";
        _context.Database.ExecuteSqlInterpolated(query);
    }

    public void DeleteTitleBookmark(string username, TitleBookmarkDTO model)
    {
        FormattableString query = $"CALL DeleteTitleBookmark({username}, {model.TitleId})";
        _context.Database.ExecuteSqlInterpolated(query);
    }

    public void CreateNameBookmark(string username, NameBookmarkDTO model)
    {
        FormattableString query = $"CALL AddNameBookmark({username}, {model.NameId}, {model.Notes})";
        _context.Database.ExecuteSqlInterpolated(query);
    }

    public IList<NameBookmarkDTO> GetNameBookmarks(string username)
    {
        var bookmarks = _context.NameBookmarks
            .Where(b => b.Username == username)
            .ToList();
        return _mapper.Map<IList<NameBookmarkDTO>>(bookmarks);
    }

    public void UpdateNameBookmarkNote(string username, NameBookmarkDTO model)
    {
        FormattableString query = $"CALL UpdateNoteNameBookmark({username}, {model.NameId}, {model.Notes})";
        _context.Database.ExecuteSqlInterpolated(query);
    }

    public void DeleteNameBookmark(string username, NameBookmarkDTO model)
    {
        FormattableString query = $"CALL DeleteNameBookmark({username}, {model.NameId})";
        _context.Database.ExecuteSqlInterpolated(query);
    }
}