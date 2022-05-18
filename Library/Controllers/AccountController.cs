using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Library.Models;
using System.Threading.Tasks;
using Library.ViewModels;
using System;

namespace Library.Controllers
{
  public class AccountController : Controller
  {
    private readonly LibraryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, LibraryContext db)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _db = db;
    }

    public ActionResult Index()
    {
      return View();
    }

    public IActionResult Register()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Register(RegisterViewModel model)
    {
      var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FullName = model.FullName };
      Room room = new Room { Name = "Living Room", Background = "room.jpg", Scale = "1" };
      Shelf shelf = new Shelf() { Room = room, Top = "120px", Left = "60px", Height = "105px", Width = "125px" };
      IdentityResult result = await _userManager.CreateAsync(user, model.Password);
      if (result.Succeeded)
      {
        Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
        if (signInResult.Succeeded)
        {
          room.User = user;
          shelf.User = user;
          _db.Rooms.Add(room);
          _db.Shelves.Add(shelf);
          _db.SaveChanges();
          return RedirectToAction("Index", "Home");
        }
        else
        {
          return View("Index", "Account");
        }
      }
      else
      {
        Console.WriteLine(result);
        return View("Index", "Account");
      }
    }

    public ActionResult Login()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login(LoginViewModel model)
    {
      Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
      if (result.Succeeded)
      {
        return RedirectToAction("Index", "Home");
      }
      else
      {
        return View("Index");
      }
    }

    public async Task<JsonResult> Validate(string email)
    {
      var currentUser = await _userManager.FindByEmailAsync(email);
      return Json(new { account = currentUser });
    }

    [HttpPost]
    public async Task<ActionResult> LogOff()
    {
      await _signInManager.SignOutAsync();
      return RedirectToAction("Index");
    }
  }
}