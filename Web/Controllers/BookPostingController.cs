using Business.Services;
using Business.Services.Interfaces;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers;

[Route("/posts")]
public class BookPostingController : Controller
{
    private readonly ILogger<BookPostingController> _logger;
    private readonly IBookPostingService _bookPostingService;
    private readonly IApplicationUserService _applicationUserService;

    public BookPostingController(ILogger<BookPostingController> logger, IBookPostingService bookPostingService, IApplicationUserService applicationUserService)
    {
        _logger = logger;
        _bookPostingService = bookPostingService;
        _applicationUserService = applicationUserService;
    }

    // post with id
    [HttpGet("{id}")]
    public IActionResult Details()
    {
        _logger.LogInformation("Getting post with id");
        var id = long.Parse(RouteData.Values["id"]!.ToString()!);
        var post = _bookPostingService.GetById(id);
        return View("Index", post);
    }

    // add posting form
    [Authorize]
    [HttpGet("add")]
    public IActionResult Add()
    {
        _logger.LogInformation("Getting add posting form");
        return View("Add");
    }

    // add posting
    [Authorize]
    [HttpPost("add")]
    public IActionResult Add(BookPosting bookPosting)
    {
        _logger.LogInformation("Adding posting");

        // get authorized user
        string userId = User.Identity!.Name!;
        _logger.LogInformation($"User {userId} tries to add a posting");
        bookPosting.OwnerId = userId;
        bookPosting.Owner = _applicationUserService.GetUserByEmail(userId)!;
        bookPosting.PostDateTime = DateTime.UtcNow;
        bookPosting.ExpireDateTime = DateTime.UtcNow.AddDays(30);

        var post = _bookPostingService.Add(bookPosting);
        return RedirectToAction("Details", new { id = post!.Id });
    }

    // my posts
    [Authorize]
    [HttpGet("/my")]
    public IActionResult MyBooks(int pageNumber = 1, int pageSize = 8, string search = "")
    {
        IQueryable<BookPosting> posts;
        string ownerId = _applicationUserService.GetUserByEmail(User.Identity!.Name!)!.Id;
        if (!string.IsNullOrEmpty(search))
        {
            posts = _bookPostingService.Search(search).Where(p => p.OwnerId == ownerId);
        }
        else
        {
            posts = _bookPostingService.GetByOwnerId(ownerId);
        }
        var model = PaginationViewModel<BookPosting>.Create(posts, pageNumber, pageSize);
        return View("MyBooks", model);
    }

    // activate
    [Authorize]
    [HttpGet("{id}/activate")]
    public IActionResult Activate()
    {
        _logger.LogInformation("Activating post");
        var id = long.Parse(RouteData.Values["id"]!.ToString()!);
        var post = _bookPostingService.GetById(id);
        if (post == null)
        {
            return NotFound();
        }
        post.ExpireDateTime = DateTime.UtcNow.AddDays(30);
        _bookPostingService.Update(id, post);
        return RedirectToAction("MyBooks");
    }

    // delete
    [Authorize]
    [HttpGet("{id}/delete")]
    public IActionResult Delete()
    {
        _logger.LogInformation("Deleting post");
        var id = long.Parse(RouteData.Values["id"]!.ToString()!);
        _bookPostingService.Delete(id);
        return RedirectToAction("MyBooks");
    }

    // edit
    [Authorize]
    [HttpGet("{id}/edit")]
    public IActionResult Edit()
    {
        _logger.LogInformation("Getting edit posting form");
        var id = long.Parse(RouteData.Values["id"]!.ToString()!);
        var post = _bookPostingService.GetById(id);
        return View("Edit", post);
    }

    // edit
    [Authorize]
    [HttpPost("{id}/edit")]
    public IActionResult Edit(BookPosting newBookPosting)
    {
        _logger.LogInformation("Editing posting");
        var id = long.Parse(RouteData.Values["id"]!.ToString()!);
        _bookPostingService.Update(id, newBookPosting);
        return RedirectToAction("MyBooks");
    }
}