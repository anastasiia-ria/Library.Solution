using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;
using System.Linq;

namespace Library.Controllers
{
  public class RoomsController : Controller
  {
    private readonly LibraryContext _db;

    public RoomsController(LibraryContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Room> model = _db.Rooms.ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Room room)
    {
      _db.Rooms.Add(room);
      _db.SaveChanges();
      return RedirectToAction("Index");
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

    public ActionResult Delete(int id)
    {
      var thisRoom = _db.Rooms.FirstOrDefault(room => room.RoomId == id);
      return View(thisRoom);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisRoom = _db.Rooms.FirstOrDefault(room => room.RoomId == id);
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