using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Library.Controllers
{
  [Authorize]
  public class RoomsController : Controller
  {
    private readonly LibraryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public RoomsController(UserManager<ApplicationUser> userManager, LibraryContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    public async Task<ActionResult> Index(int roomId)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      ViewBag.PrevRoomId = roomId;
      ViewBag.Books = _db.Books.Where(book => book.User == currentUser).ToList();
      List<Room> model = _db.Rooms.Where(entry => entry.User.Id == currentUser.Id).ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Microsoft.AspNetCore.Http.IFormCollection form)
    {
      Room room = new Room() { Name = form["Name"] };
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      room.User = currentUser;
      Shelf shelf = new Shelf() { Room = room, User = currentUser };
      _db.Shelves.Add(shelf);
      _db.Rooms.Add(room);
      _db.SaveChanges();
      return RedirectToAction("Index", "Rooms", new { roomId = room.RoomId });
    }

    public ActionResult Details(int id)
    {
      Room thisRoom = _db.Rooms.FirstOrDefault(room => room.RoomId == id);
      return View(thisRoom);
    }
    public ActionResult Edit(int id)
    {
      var thisRoom = _db.Rooms.FirstOrDefault(room => room.RoomId == id);
      return View(thisRoom);
    }

    [HttpPost]
    public ActionResult Edit(Room room)
    {
      _db.Entry(room).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = room.RoomId });
    }

    [HttpPost]
    public ActionResult Delete(int id)
    {
      var thisRoom = _db.Rooms.FirstOrDefault(room => room.RoomId == id);
      var shelvesId = _db.Shelves.Where(shelf => shelf.RoomId == id).Select(shelf => shelf.ShelfId).ToList();
      foreach (int ShelfId in shelvesId)
      {
        _db.Books.Where(book => book.Shelf.ShelfId == ShelfId).ToList().ForEach(book => book.Shelf = null);
      }

      var shelves = _db.Shelves.Where(shelf => shelf.RoomId == id).ToList();
      foreach (var shelf in shelves)
      {
        _db.Shelves.Remove(shelf);
      }

      _db.Books.Where(book => book.Room.RoomId == id).ToList().ForEach(book => book.Room = null);
      _db.Rooms.Remove(thisRoom);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult Search(string parameter)
    {
      var roomList = _db.Rooms.AsQueryable();
      roomList = roomList.Where(room => room.Name.Contains(parameter));
      var search = roomList.ToList();
      ModelState.Clear();
      return View("Index", search);
    }
  }
}