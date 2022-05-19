using System.Collections.Generic;

namespace Library.Models
{
  public class Room
  {
    public Room()
    {
      this.Books = new HashSet<Book>();

      this.Shelves = new HashSet<Shelf>();
    }
    public int RoomId { get; set; }
    public string Name { get; set; }
    public string Width { get; set; }
    public string Background { get; set; }
    public virtual ICollection<Shelf> Shelves { get; set; }
    public virtual ICollection<Book> Books { get; set; }
    public virtual ApplicationUser User { get; set; }
  }
}