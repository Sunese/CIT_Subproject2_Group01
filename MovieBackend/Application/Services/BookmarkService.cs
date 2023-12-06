using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Context;
using Application.Models;
using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public enum OrderBy
{
    Alphabetical,
    Rating,
    ReleaseDate,
    Created
}

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

    public (IList<TitleBookmarkDTO>, int) GetTitleBookmarks(string username, OrderBy orderBy, int page, int pageSize)
    {
        IEnumerable<TitleBookmark> bookmarks = orderBy switch
        {
            OrderBy.Alphabetical => _context.TitleBookmarks
                                .Include(tb => tb.Title)
                                .Where(tb => tb.Username == username)
                                .OrderBy(tb => tb.Title.PrimaryTitle),
            OrderBy.Rating => _context.TitleBookmarks
                                .Include(tb => tb.Title)
                                .ThenInclude(t => t.TitleRating)
                                .Where(tb => tb.Username == username)
                                .OrderByDescending(tb => tb.Title.TitleRating.AverageRating),
            OrderBy.ReleaseDate => _context.TitleBookmarks
                                .Include(tb => tb.Title)
                                .Where(tb => tb.Username == username)
                                .OrderBy(tb => tb.Title.Released),
            OrderBy.Created => _context.TitleBookmarks
                                .Include(tb => tb.Title)
                                .Where(tb => tb.Username == username)
                                .OrderBy(tb => tb.Timestamp),
            _ => throw new NotImplementedException(),
        };

        var paged = bookmarks
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToList();

        return (_mapper.Map<IList<TitleBookmarkDTO>>(paged), bookmarks.Count());
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

    public bool TitleBookmarkExists(string username, string titleId)
    {
        return _context.TitleBookmarks.Any(tb => tb.Username == username && tb.TitleId == titleId);
    }

    public bool TryGetTitleBookmark(string username, string titleId, out TitleBookmarkDTO? foundBookmark)
    {
        var bookmark = _context.TitleBookmarks.FirstOrDefault(tb => tb.Username == username && tb.TitleId == titleId);
        if (bookmark == null)
        {
            foundBookmark = null;
            return false;
        }
        foundBookmark = _mapper.Map<TitleBookmarkDTO>(bookmark);
        return true;
    }

    public void CreateNameBookmark(string username, NameBookmarkDTO model)
    {
        FormattableString query = $"CALL AddNameBookmark({username}, {model.NameId}, {model.Notes})";
        _context.Database.ExecuteSqlInterpolated(query);
    }

    public (IList<NameBookmarkDTO>, int) GetNameBookmarks(string username, OrderBy orderBy, int page, int pageSize)
    {
        IEnumerable<NameBookmark> bookmarks = orderBy switch
        {
            OrderBy.Alphabetical => _context.NameBookmarks
                .Include(n => n.Name)
                .Where(nb => nb.Username == username)
                .OrderBy(nb => nb.Name.PrimaryName),
            OrderBy.Created => _context.NameBookmarks
                .Where(tb => tb.Username == username)
                .OrderBy(tb => tb.Timestamp),
            _ => throw new NotImplementedException(),
        };

        var paged = bookmarks
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToList();

        return (_mapper.Map<IList<NameBookmarkDTO>>(paged), bookmarks.Count());
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

    public bool NameBookmarkExists(string username, string nameId)
    {
        return _context.NameBookmarks.Any(nb => nb.Username == username && nb.NameId == nameId);
    }

    public bool TryGetNameBookmark(string username, string nameId, out NameBookmarkDTO? foundBookmark)
    {
        var bookmark = _context.NameBookmarks.FirstOrDefault(nb => nb.Username == username && nb.NameId == nameId);
        if (bookmark == null)
        {
            foundBookmark = null;
            return false;
        }
        foundBookmark = _mapper.Map<NameBookmarkDTO>(bookmark);
        return true;
    }

}