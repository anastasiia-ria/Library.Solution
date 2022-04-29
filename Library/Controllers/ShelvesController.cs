using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;
using System.Linq;

namespace Library.Controllers
{
  public class ShelvesController : Controller
  {
    private readonly LibraryContext _db;

    public ShelvesController(LibraryContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Shelf> model = _db.Shelves.ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Shelf shelf)
    {
      _db.Shelves.Add(shelf);
      _db.SaveChanges();
      return RedirectToAction("Index");
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

    public ActionResult Delete(int id)
    {
      var thisShelf = _db.Shelves.FirstOrDefault(shelf => shelf.ShelfId == id);
      return View(thisShelf);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisShelf = _db.Shelves.FirstOrDefault(shelf => shelf.ShelfId == id);
      _db.Shelves.Remove(thisShelf);
      _db.SaveChanges();
      return RedirectToAction("Index");
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