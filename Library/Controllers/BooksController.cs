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
      var array = Book.GetBooks(general, title, authors, publisher, isbn, startIndex);
      var size = array[1];
      var allBooks = array[0];
      return Json(new { Books = allBooks, Size = size });
    }

    public async Task<ActionResult> Index()
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      ViewBag.Count = _db.Books.Where(entry => entry.User.Id == currentUser.Id).Count();
      List<Book> model = _db.Books.Where(entry => entry.User.Id == currentUser.Id).OrderBy(book => book.Title).Take(8).ToList();
      return View(model);
    }

    public async Task<JsonResult> Pagination(int skip, int shelfId)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      Shelf shelf = _db.Shelves.FirstOrDefault(shelf => shelf.ShelfId == shelfId);
      int size = _db.Books.Where(book => book.User == currentUser && (book.Shelf == null || book.Shelf != shelf)).Count();
      List<Book> books = _db.Books.Where(book => book.User == currentUser && (book.Shelf == null || book.Shelf != shelf)).OrderBy(book => book.Title).Skip(skip).Take(8).ToList();
      return Json(new { books = books, size = size });
    }
    public JsonResult Create(string id)
    {
      var book = Book.GetDetails(id);
      return Json(new { book = book });
    }

    [HttpPost]
    public async Task<ActionResult> Create(Book book)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      if (!_db.Books.Any(b => b.ImgID == book.ImgID && b.User == currentUser))
      {
        book.User = currentUser;
        _db.Books.Add(book);
        _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }
    [HttpPost]
    public JsonResult AddLocation(int id, int shelfId, int roomId)
    {
      Book book = _db.Books.FirstOrDefault(book => book.BookId == id);
      Shelf shelf = _db.Shelves.FirstOrDefault(shelf => shelf.ShelfId == shelfId);
      Room room = _db.Rooms.FirstOrDefault(room => room.RoomId == roomId);
      book.Shelf = shelf;
      book.Room = room;
      _db.SaveChanges();
      return Json(new { img = book.ImgID });
    }
    [HttpPost]
    public JsonResult RemoveLocation(int id)
    {
      Book book = _db.Books.FirstOrDefault(book => book.BookId == id);
      Book newBook = new Book() { Title = book.Title, Authors = book.Authors, Subtitle = book.Subtitle, Publisher = book.Publisher, Description = book.Description, PublishedDate = book.PublishedDate, ISBN_10 = book.ISBN_10, ISBN_13 = book.ISBN_13, ImgID = book.ImgID, PageCount = book.PageCount, Status = book.Status, Categories = book.Categories, Language = book.Language, Rating = book.Rating, User = book.User };
      _db.Books.Remove(book);
      _db.Books.Add(newBook);
      _db.SaveChanges();
      return Json(new { });
    }
    public JsonResult Details(int id)
    {
      Book thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      return Json(new { thisBook = thisBook });
    }

    [HttpPost]
    public ActionResult Edit(Book book)
    {
      _db.Entry(book).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }


    [HttpPost]
    public ActionResult Delete(int id)
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