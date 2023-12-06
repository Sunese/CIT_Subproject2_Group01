using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;

namespace Application.Services;

public interface IBookmarkService
{
    
    bool TryGetNameBookmark(string username, string nameId, out NameBookmarkDTO? foundBookmark);
    (IList<NameBookmarkDTO>, int) GetNameBookmarks(string username, OrderBy orderBy, int page, int pageSize);
    void CreateNameBookmark(string username, NameBookmarkDTO model);
    void UpdateNameBookmarkNote(string username, NameBookmarkDTO model);
    void DeleteNameBookmark(string username, NameBookmarkDTO model);
    bool NameBookmarkExists(string username, string nameId);

    bool TryGetTitleBookmark(string username, string titleId, out TitleBookmarkDTO? foundBookmark);
    (IList<TitleBookmarkDTO>, int) GetTitleBookmarks(string username, OrderBy orderBy, int page, int pageSize);
    void CreateTitleBookmark(string username, TitleBookmarkDTO model);
    void UpdateTitleBookmarkNote(string username, TitleBookmarkDTO model);
    void DeleteTitleBookmark(string username, TitleBookmarkDTO model);
    bool TitleBookmarkExists(string username, string titleId);
}