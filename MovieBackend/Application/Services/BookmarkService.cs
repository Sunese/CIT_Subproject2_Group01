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

    public IList<TitleBookmarkDTO> GetTitleBookmarks(string username, OrderBy orderBy, int count)
    {
        IList<TitleBookmark> bookmarks = orderBy switch
        {
            OrderBy.Alphabetical => _context.TitleBookmarks
                                .Include(tb => tb.Title)
                                .Where(tb => tb.Username == username)
                                .OrderBy(tb => tb.Title.PrimaryTitle)
                                .Take(count)
                                .ToList(),
            OrderBy.Rating => _context.TitleBookmarks
                                .Include(tb => tb.Title)
                                .ThenInclude(t => t.TitleRating)
                                .Where(tb => tb.Username == username)
                                .OrderByDescending(tb => tb.Title.TitleRating.AverageRating)
                                .Take(count)
                                .ToList(),
            OrderBy.ReleaseDate => _context.TitleBookmarks
                                .Include(tb => tb.Title)
                                .Where(tb => tb.Username == username)
                                .OrderBy(tb => tb.Title.Released)
                                .Take(count)
                                .ToList(),
            OrderBy.Created => _context.TitleBookmarks
                                .Include(tb => tb.Title)
                                .Where(tb => tb.Username == username)
                                .OrderBy(tb => tb.Timestamp)
                                .Take(count)
                                .ToList(),
            _ => throw new NotImplementedException(),
        };
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

    public IList<NameBookmarkDTO> GetNameBookmarks(string username, OrderBy orderBy, int count)
    {
        IList<NameBookmark> bookmarks = orderBy switch
        {
            OrderBy.Alphabetical => _context.NameBookmarks
                .Include(n => n.Name)
                .Where(nb => nb.Username == username)
                .OrderBy(nb => nb.Name.PrimaryName)
                .Take(count)
                .ToList(),
            OrderBy.Created => _context.NameBookmarks
                .Where(tb => tb.Username == username)
                .OrderBy(tb => tb.Timestamp)
                .Take(count)
                .ToList(),
            _ => throw new NotImplementedException(),
        };
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