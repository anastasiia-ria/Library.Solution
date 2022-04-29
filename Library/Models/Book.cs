using System.Collections.Generic;

namespace Library.Models
{
  public class Book
  {
    public int BookId { get; set; }
    public string Title { get; set; }
    public string Authors { get; set; }
    public string Publisher { get; set; }
    public string PublishedDate { get; set; }
    public string Description { get; set; }
    public string ISBN_10 { get; set; }
    public string ISBN_13 { get; set; }
    public string ImgID { get; set; }
    public string PageCount { get; set; }
    public int ShelfId { get; set; }
    public virtual Shelf Shelf { get; set; }
    public int RoomId { get; set; }
    public virtual Room Room { get; set; }
    public virtual ApplicationUser User { get; set; }
  }
}