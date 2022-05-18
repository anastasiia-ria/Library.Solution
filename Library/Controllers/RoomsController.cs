using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Library.Models;
using Library.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using System.Security.Claims;
using System;
using System.IO;
namespace Library.Controllers
{
  [Authorize]
  public class RoomsController : Controller
  {
    private readonly LibraryContext _db;
    private readonly IWebHostEnvironment webHostEnvironment;
    private readonly UserManager<ApplicationUser> _userManager;

    public RoomsController(IWebHostEnvironment hostEnvironment, UserManager<ApplicationUser> userManager, LibraryContext db)
    {
      webHostEnvironment = hostEnvironment;
      _userManager = userManager;
      _db = db;
    }
    public async Task<ActionResult> Index(int id)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      ViewBag.room = _db.Rooms.FirstOrDefault(room => room.RoomId == id) ?? _db.Rooms.FirstOrDefault(room => room.User == currentUser);
      ViewBag.image = ViewBag.room.Background;
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
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      Room room = new Room() { Name = form["Name"], User = currentUser, Background = "room.jpg", Scale = "1" };
      Shelf shelf = new Shelf() { Room = room, User = currentUser, Top = "120px", Left = "60px", Height = "105px", Width = "125px" };
      _db.Shelves.Add(shelf);
      _db.Rooms.Add(room);
      _db.SaveChanges();
      return RedirectToAction("Index", "Rooms", new { id = room.RoomId });
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
    public JsonResult Scale(int id, string scale)
    {
      Room room = _db.Rooms.FirstOrDefault(room => room.RoomId == id);
      room.Scale = scale;
      _db.SaveChanges();
      return Json(new { });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult AddBackground(BackgroundViewModel model)
    {
      Console.WriteLine("add");
      if (ModelState.IsValid)
      {
        string uniqueFileName = UploadedFile(model);
        Room room = _db.Rooms.FirstOrDefault(room => room.RoomId == model.RoomId);
        room.Background = uniqueFileName;
        _db.SaveChanges();
        return RedirectToAction("Index", "Rooms", new { id = room.RoomId });
      }
      return View();
    }

    private string UploadedFile(BackgroundViewModel model)
    {
      string uniqueFileName = null;

      if (model.Background != null)
      {
        string rootFolderPath = Path.Combine(webHostEnvironment.WebRootPath, "img/background");
        string[] files = Directory.GetFiles(rootFolderPath, @"*RoomId_" + model.RoomId + "*");
        foreach (string f in files)
        {
          System.IO.File.Delete(f);
        }
        string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "img/background");
        uniqueFileName = Guid.NewGuid().ToString() + "_RoomId_" + model.RoomId + model.Background.FileName;
        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
          model.Background.CopyTo(fileStream);
        }
      }
      return uniqueFileName;
    }

    public async Task<JsonResult> Filter(string title, string authors, string publisher, string isbn, string status)
    {
      Console.WriteLine(title + authors + publisher + isbn + status);
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      var filter = _db.Books.Where(book => book.User == currentUser);
      if (title != null)
      {
        filter = filter.Where(book => book.Title.ToLower().Contains(title));
      }
      if (authors != null)
      {
        filter = filter.Where(book => book.Authors.ToLower().Contains(authors));
      }
      if (publisher != null)
      {
        filter = filter.Where(book => book.Publisher.ToLower().Contains(publisher));
      }
      if (isbn != null)
      {
        filter = filter.Where(book => book.ISBN_10.ToLower().Contains(isbn) || book.ISBN_13.ToLower().Contains(isbn));
      }
      if (status != null)
      {
        filter = filter.Where(book => book.Status.ToLower().Contains(status));
      }
      var books = filter.ToList();
      return Json(new { books = books });
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
      Console.WriteLine("Delete");
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