using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;

namespace Application.Services;

public interface IBookmarkService
{
    IList<NameBookmarkDTO> GetNameBookmarks(string username);
    void CreateNameBookmark(string username, NameBookmarkDTO model);
    void UpdateNameBookmarkNote(string username, NameBookmarkDTO model);
    void DeleteNameBookmark(string username, NameBookmarkDTO model);

    IList<TitleBookmarkDTO> GetTitleBookmarks(string username);
    void CreateTitleBookmark(string username, TitleBookmarkDTO model);
    void UpdateTitleBookmarkNote(string username, TitleBookmarkDTO model);
    void DeleteTitleBookmark(string username, TitleBookmarkDTO model);
}