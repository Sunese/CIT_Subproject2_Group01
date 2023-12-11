using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;

namespace Application.Services;

public interface IBookmarkService
{
    
    bool TryGetNameBookmark(string username, string nameID, out NameBookmarkDTO? foundBookmark);
    (IList<NameBookmarkDTO>, int) GetNameBookmarks(string username, OrderBy orderBy, int page, int pageSize);
    void CreateNameBookmark(string username, NameBookmarkDTO model);
    void UpdateNameBookmarkNote(string username, string nameID, string newNotes);
    void DeleteNameBookmark(string username, NameBookmarkDTO model);
    bool NameBookmarkExists(string username, string nameID);

    bool TryGetTitleBookmark(string username, string titleID, out TitleBookmarkDTO? foundBookmark);
    (IList<TitleBookmarkDTO>, int) GetTitleBookmarks(string username, OrderBy orderBy, int page, int pageSize);
    void CreateTitleBookmark(string username, TitleBookmarkDTO model);
    void UpdateTitleBookmarkNote(string username, string titleID, string newNote);
    void DeleteTitleBookmark(string username, TitleBookmarkDTO model);
    bool TitleBookmarkExists(string username, string titleID);
}