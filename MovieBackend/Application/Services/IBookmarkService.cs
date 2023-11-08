using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;

namespace Application.Services;

public interface IBookmarkService
{
    (IList<NameBookmarkDTO>, int) GetNameBookmarks(string username, OrderBy orderBy, int page, int pageSize);
    void CreateNameBookmark(string username, NameBookmarkDTO model);
    void UpdateNameBookmarkNote(string username, NameBookmarkDTO model);
    void DeleteNameBookmark(string username, NameBookmarkDTO model);
    bool NameBookmarkExists(string username, string nameId);

    (IList<TitleBookmarkDTO>, int) GetTitleBookmarks(string username, OrderBy orderBy, int page, int pageSize);
    void CreateTitleBookmark(string username, TitleBookmarkDTO model);
    void UpdateTitleBookmarkNote(string username, TitleBookmarkDTO model);
    void DeleteTitleBookmark(string username, TitleBookmarkDTO model);
    bool TitleBookmarkExists(string username, string titleId);
}