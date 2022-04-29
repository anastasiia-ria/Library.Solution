using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Library.Models;

namespace Library.Controllers
{
  public class HomeController : Controller
  {
    private readonly LibraryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<HomeController> _logger;
    public HomeController(UserManager<ApplicationUser> userManager, LibraryContext db, ILogger<HomeController> logger)
    {
      _userManager = userManager;
      _db = db;
      _logger = logger;
    }
    [HttpGet("/")]
    public IActionResult Index()
    {
      return View();
    }

    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

  }
}