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
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      List<Book> model = _db.Books.Where(entry => entry.User.Id == currentUser.Id).OrderBy(book => book.Title).Take(8).ToList();
      return View(model);
    }
    //     public async Task<ActionResult> Index(int page)
    // {
    //   int count = _db.Books.Count();
    //   int perPage = 2;
    //   int maxPage = (int)Math.Ceiling(((double)count) / perPage);
    //   int Page = 1;
    //   if (page == 0)
    //   {
    //     Page = 1;
    //   }
    //   else if (page > maxPage)
    //   {
    //     Page = maxPage;
    //   }
    //   else
    //   {
    //     Page = page;
    //   }
    //   ViewBag.Page = Page;
    //   var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //   var currentUser = await _userManager.FindByIdAsync(userId);
    //   List<Book> model = _db.Books.Where(entry => entry.User.Id == currentUser.Id).OrderBy(book => book.Title).Skip(perPage * (Page - 1)).Take(perPage).ToList();
    //   return View(model);
    // }

    public async Task<JsonResult> Pagination(int skip)
    {
      int size = _db.Books.Count();
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      List<Book> books = _db.Books.Where(entry => entry.User.Id == currentUser.Id).OrderBy(book => book.Title).Skip(skip).Take(8).ToList();
      return Json(new { books = books, size = size });
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
      if (!_db.Books.Any(book => book.ImgID == form["ImgID"].ToString()))
      {
        var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var currentUser = await _userManager.FindByIdAsync(userId);
        Book book = new Book { Title = form["Title"], Authors = form["Authors"], Description = form["Description"], ISBN_10 = form["ISBN_10"], ISBN_13 = form["ISBN_13"], Publisher = form["Publisher"], PublishedDate = form["PublishedDate"], PageCount = form["PageCount"], Status = form["Status"], ImgID = form["ImgID"] };
        book.User = currentUser;
        _db.Books.Add(book);
        _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }
    [HttpPost]
    public ActionResult AddLocation(int id, int shelfId, int roomId)
    {
      Book book = _db.Books.FirstOrDefault(book => book.BookId == id);
      Shelf shelf = _db.Shelves.FirstOrDefault(shelf => shelf.ShelfId == shelfId);
      Room room = _db.Rooms.FirstOrDefault(room => room.RoomId == roomId);
      book.Shelf = shelf;
      book.Room = room;
      _db.SaveChanges();
      return RedirectToAction("Index", "Rooms", new { roomId = roomId });
    }
    public JsonResult Details(int id)
    {
      Book thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
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