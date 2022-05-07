namespace Library.Models
{
  public class Pagination
  {
    public int CurrentPage { get; set; } = 1;
    public int Count { get; set; }
    public int PageSize { get; set; } = 8;
    public int TotalPages { get; set; }
    public bool ShowPrevious => CurrentPage > 1;
    public bool ShowNext => CurrentPage < TotalPages;
    public bool ShowFirst => CurrentPage != 1;
    public bool ShowLast => CurrentPage != TotalPages;
  }
}