using System.Diagnostics;
using Business.Services;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers;

[Route("/")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IBookPostingService _bookPostingService;

    public HomeController(ILogger<HomeController> logger, IBookPostingService bookPostingService)
    {
        _logger = logger;
        _bookPostingService = bookPostingService;
    }

    [HttpGet]
    public ActionResult Index(int pageNumber = 1, int pageSize = 8, string search = "")
    {
        IQueryable<BookPosting> posts;
        if (!string.IsNullOrEmpty(search))
        {
            posts = _bookPostingService.Search(search);
        }
        else
        {
            posts = _bookPostingService.Get();
        }
        // filter posts to be active
        posts = posts.Where(p => p.ExpireDateTime > DateTime.UtcNow).OrderByDescending(p => p.PostDateTime);
        var model = PaginationViewModel<BookPosting>.Create(posts, pageNumber, pageSize);
        return View("Index", model);
    }

    [HttpGet("privacy")]
    public IActionResult Privacy()
    {
        return View("Privacy");
    }

    [HttpGet("error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
