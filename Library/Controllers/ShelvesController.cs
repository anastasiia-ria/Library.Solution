using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Library.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
namespace Library.Controllers
{
  [Authorize]
  public class ShelvesController : Controller
  {
    private readonly LibraryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public ShelvesController(UserManager<ApplicationUser> userManager, LibraryContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    public async Task<ActionResult> Index()
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      List<Shelf> model = _db.Shelves.Where(entry => entry.User.Id == currentUser.Id).ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public async Task<JsonResult> Create(int id)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      Room room = _db.Rooms.FirstOrDefault(room => room.RoomId == id);
      Shelf shelf = new Shelf() { Room = room, User = currentUser, Top = "120px", Left = "60px", Height = "105px", Width = "225px" };
      _db.Shelves.Add(shelf);
      _db.SaveChanges();
      return Json(new { shelf = shelf });
    }
    [HttpPost]
    public JsonResult Drag(int id, string top, string left, string width, string height, string roomWidth)
    {
      Shelf shelf = _db.Shelves.FirstOrDefault(shelf => shelf.ShelfId == id);
      Room room = _db.Rooms.FirstOrDefault(room => room.RoomId == shelf.Room.RoomId);
      shelf.Top = top;
      shelf.Left = left;
      shelf.Width = width;
      shelf.Height = height;
      room.Width = roomWidth;
      _db.SaveChanges();
      return Json(new { });
    }

    public ActionResult Details(int id)
    {
      Shelf thisShelf = _db.Shelves.FirstOrDefault(shelf => shelf.ShelfId == id);
      return View(thisShelf);
    }
    public ActionResult Edit(int id)
    {
      var thisShelf = _db.Shelves.FirstOrDefault(shelf => shelf.ShelfId == id);
      return View(thisShelf);
    }

    [HttpPost]
    public ActionResult Edit(Shelf shelf)
    {
      _db.Entry(shelf).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = shelf.ShelfId });
    }
    [HttpPost]
    public ActionResult Delete(int id)
    {
      var thisShelf = _db.Shelves.FirstOrDefault(shelf => shelf.ShelfId == id);
      _db.Books.Where(book => book.Shelf.ShelfId == id).ToList().ForEach(book => book.Shelf = null);
      _db.Shelves.Remove(thisShelf);
      _db.SaveChanges();
      return RedirectToAction("Index", "Rooms", new { id = thisShelf.RoomId });
    }

    [HttpPost]
    public ActionResult Search(string parameter)
    {
      var shelfList = _db.Shelves.AsQueryable();

      shelfList = shelfList.Where(shelf => shelf.Name.Contains(parameter));
      var search = shelfList.ToList();
      ModelState.Clear();
      return View("Index", search);
    }
  }
}