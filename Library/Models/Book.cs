using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
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
    public string Status { get; set; }
    public virtual Shelf Shelf { get; set; }
    public virtual Room Room { get; set; }
    public virtual ApplicationUser User { get; set; }
    public static List<Book> GetBooks(string general, string title, string authors, string publisher, string isbn)
    {
      var apiCallTask = ApiHelper.GetAll(general, title, authors, publisher, isbn);
      var result = apiCallTask.Result;
      var resultParse = JObject.Parse(result)["items"];
      List<Book> bookList = new List<Book>();
      foreach (var item in resultParse)
      {
        JObject info = item["volumeInfo"].ToObject<JObject>();
        var Title = info.ContainsKey("title") ? info["title"].ToString() : "";
        var Authors = info.ContainsKey("authors") ? String.Join(", ", info["authors"].ToObject<string[]>()) : "";
        bookList.Add(new Book { Title = Title, Authors = Authors, ImgID = item["id"].ToString() });
      }
      return bookList;
    }
    public static Book GetDetails(string id)
    {
      var apiCallTask = ApiHelper.Get(id);
      var result = apiCallTask.Result;
      var resultParse = JValue.Parse(result);
      var info = resultParse["volumeInfo"];
      Book book = new Book { Title = info["title"].ToString(), Authors = String.Join(", ", info["authors"].ToObject<string[]>()), Publisher = info["publisher"].ToString(), PublishedDate = info["publishedDate"].ToString(), Description = info["description"].ToString(), ISBN_10 = info["industryIdentifiers"][1]["identifier"].ToString(), ISBN_13 = info["industryIdentifiers"][0]["identifier"].ToString(), PageCount = info["pageCount"].ToString(), ImgID = resultParse["id"].ToString() };
      return book;
    }
    public static void Post(Book book)
    {
      string jsonBook = JsonConvert.SerializeObject(book);
      var apiCallTask = ApiHelper.Post(jsonBook);
    }

    public static void Put(Book book)
    {
      string jsonBook = JsonConvert.SerializeObject(book);
      var apiCallTask = ApiHelper.Put(book.BookId, jsonBook);
    }

    public static void Delete(int id)
    {
      var apiCallTask = ApiHelper.Delete(id);
    }
  }
}