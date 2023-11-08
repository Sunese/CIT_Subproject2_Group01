using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers;

public class MovieBaseController : ControllerBase
{
    private readonly LinkGenerator _linkGenerator;

    public MovieBaseController(LinkGenerator linkGenerator)
    {
        _linkGenerator = linkGenerator;
    }

    protected object Paging<T>(IEnumerable<T> items, int total, int page, int pageSize, string endpointName)
    {

        var numPages = (int)Math.Ceiling(total / (double)pageSize);
        var next = page < numPages - 1
            ? GetUrl(endpointName, new { page = page + 1, pageSize })
            : null;
        var prev = page > 0
            ? GetUrl(endpointName, new { page = page - 1, pageSize })
            : null;

        var cur = GetUrl(endpointName, new { page, pageSize });

        return new
        {
            Total = total,
            NumberOfPages = numPages,
            Next = next,
            Prev = prev,
            Current = cur,
            Items = items
        };
    }

    // Same as the other Paging(), but with a RouteValueDictionary
    // such that specific template+values can be used
    // when creating a paged result for something resource-specific
    protected object Paging<T>(IEnumerable<T> items, int total, int page, int pageSize, string endpointName, RouteValueDictionary routeValueDictionary)
    {
        var numPages = (int)Math.Ceiling(total / (double)pageSize);
        string? next;
        string? prev;
        if (page < numPages - 1)
        {
            routeValueDictionary["page"] = page + 1;
            routeValueDictionary["pageSize"] = pageSize;
            next = GetUrl(endpointName, routeValueDictionary);
        }
        else next = null;
        if (page > 0)
        {
            routeValueDictionary["page"] = page - 1;
            routeValueDictionary["pageSize"] = pageSize;
            prev = GetUrl(endpointName, routeValueDictionary);
        }
        else prev = null;

        routeValueDictionary["page"] = page;
        routeValueDictionary["pageSize"] = pageSize;
        var cur = GetUrl(endpointName, routeValueDictionary);

        return new
        {
            Total = total,
            NumberOfPages = numPages,
            Next = next,
            Prev = prev,
            Current = cur,
            Items = items
        };
    }

    protected string GetUrl(string name, object values)
    {
        return _linkGenerator.GetUriByName(HttpContext, name, values) ?? "Not specified";
    }

    protected string GetUrl(string name, RouteValueDictionary values)
    {
        return _linkGenerator.GetUriByName(HttpContext, name, values) ?? "Not specified";
    }

    protected bool OwnsResource(string username)
    {
        if (HttpContext.User.Identity is null)
        {
            return false;
        }
        else if (HttpContext.User.Identity.Name.IsNullOrEmpty())
        {
            return false;
        }

        var authenticatedUsername = HttpContext.User.Identity.Name.ToLower();
        if (authenticatedUsername.ToLower() != username.ToLower())
        {
            return false;
        }

        return true;
    }

    protected bool IsSignedIn()
    {
        return HttpContext.User.Identity.IsAuthenticated;
    }

    protected bool IsAdmin()
    {
        return HttpContext.User.IsInRole("Admin");
    }
}