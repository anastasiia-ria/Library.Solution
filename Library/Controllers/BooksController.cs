using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
namespace Library.Controllers
{
  [Authorize]
  public class BooksController : Controller
  {
    private readonly LibraryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public BooksController(UserManager<ApplicationUser> userManager, LibraryContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    public JsonResult Search(string general, string title, string authors, string publisher, string isbn, int startIndex)
    {
      Console.WriteLine(general);
      var array = Book.GetBooks(general, title, authors, publisher, isbn, startIndex);
      var size = array[1];
      var allBooks = array[0];
      Console.WriteLine(allBooks);
      Console.WriteLine(size);
      return Json(new { Books = allBooks, Size = size });
    }

    public async Task<ActionResult> Index()
    {
      ViewBag.Pagination = new Pagination();
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      List<Book> model = _db.Books.Where(entry => entry.User.Id == currentUser.Id).ToList();
      return View(model);
    }

    public JsonResult Create(string id)
    {
      ViewBag.ShelfId = new SelectList(_db.Shelves, "ShelfId", "ShelfId");
      ViewBag.RoomId = new SelectList(_db.Rooms, "RoomId", "RoomId");
      var book = Book.GetDetails(id);
      return Json(new { book = book });
    }

    [HttpPost]
    public async Task<ActionResult> Create(Microsoft.AspNetCore.Http.IFormCollection form)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      Book book = new Book { Title = form["Title"], Authors = form["Authors"], Description = form["Description"], ISBN_10 = form["ISBN_10"], ISBN_13 = form["ISBN_13"], Publisher = form["Publisher"], PublishedDate = form["PublishedDate"], PageCount = form["PageCount"], Status = form["Status"], ImgID = form["ImgId"] };
      book.User = currentUser;
      _db.Books.Add(book);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    [HttpPost]
    public async Task<ActionResult> AddLocation(Microsoft.AspNetCore.Http.IFormCollection form)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      Shelf shelf = _db.Shelves.FirstOrDefault(shelf => shelf.ShelfId == Int32.Parse(form["ShelfId"]));
      Room room = _db.Rooms.FirstOrDefault(room => room.RoomId == Int32.Parse(form["RoomId"]));
      Book book = new Book { Title = form["Title"], Authors = form["Authors"], Shelf = shelf, Room = room };
      book.User = currentUser;
      _db.Books.Add(book);
      _db.SaveChanges();
      return RedirectToAction("Index", "Rooms", new { roomId = room.RoomId });
    }
    public JsonResult Details(int id)
    {
      // IEnumerable<Book> thisBook = new List<Book>();
      Book thisBook = _db.Books.FirstOrDefault(b => b.BookId == id);
      return Json(new { thisBook = thisBook });
    }
    public ActionResult Edit(int id)
    {
      var thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      return View(thisBook);
    }

    [HttpPost]
    public ActionResult Edit(Book book)
    {
      _db.Entry(book).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = book.BookId });
    }

    public ActionResult Delete(int id)
    {
      var thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      return View(thisBook);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      _db.Books.Remove(thisBook);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult Search(string parameter)
    {
      var bookList = _db.Books.AsQueryable();

      bookList = bookList.Where(book => book.Title.Contains(parameter));
      var search = bookList.ToList();
      ModelState.Clear();
      return View("Index", search);
    }
  }
}