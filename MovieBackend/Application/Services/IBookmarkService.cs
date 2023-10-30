using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;

namespace Application.Services;

public interface IBookmarkService
{
    void CreateNameBookmark(string username, NameBookmarkDTO model);
    void UpdateNameBookmark(string username, NameBookmarkDTO model);
    void DeleteNameBookmark(string username, NameBookmarkDTO model);

    void CreateTitleBookmark(string username, TitleBookmarkDTO model);
    void UpdateTitleBookmark(string username, TitleBookmarkDTO model);
    void DeleteTitleBookmark(string username, TitleBookmarkDTO model);
}