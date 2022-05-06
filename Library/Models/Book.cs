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
    public static List<Book> GetBooks(string search, string isbn)
    {
      var apiCallTask = ApiHelper.GetAll(search, isbn);
      var result = apiCallTask.Result;
      // var resultParse = JValue.Parse(result)["items"][0]["volumeInfo"];
      var resultParse = JValue.Parse(result)["items"];
      List<Book> bookList = new List<Book>();

      foreach (var item in resultParse)
      {
        var info = item["volumeInfo"];
        bookList.Add(new Book { Title = info["title"].ToString(), Authors = String.Join(", ", info["authors"].ToObject<string[]>()), ImgID = item["id"].ToString() });
      }

      return bookList;
    }
    public static Book GetDetails(string id)
    {
      var apiCallTask = ApiHelper.Get(id);
      var result = apiCallTask.Result;
      Console.WriteLine(result);
      // bookList.Add(new Book { Title = info["title"].ToString(), Authors = String.Join(", ", info["authors"].ToObject<string[]>()), Publisher =  info["publisher"].ToString(), PublishedDate =  info["publishedDate"].ToString(), });
      JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(result);
      Book book = JsonConvert.DeserializeObject<Book>(jsonResponse.ToString());
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