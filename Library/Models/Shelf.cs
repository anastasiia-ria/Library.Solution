using System.Collections.Generic;

namespace Library.Models
{
  public class Shelf
  {
    public Shelf()
    {
      this.Books = new HashSet<Book>();
    }

    public int ShelfId { get; set; }
    public string Name { get; set; }
    public string Left { get; set; }
    public string Top { get; set; }
    public string Height { get; set; }
    public string Width { get; set; }
    public int RoomId { get; set; }
    public virtual Room Room { get; set; }
    public virtual ICollection<Book> Books { get; set; }
    public virtual ApplicationUser User { get; set; }
  }
}