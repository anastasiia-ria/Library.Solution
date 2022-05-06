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
    public static Book GetBooks(string isbn)
    {
      var apiCallTask = ApiHelper.GetAll(isbn);
      var result = apiCallTask.Result;
      var resultParse = JValue.Parse(result)["items"][0]["volumeInfo"];

      Book book = new Book { Title = resultParse["title"].ToString(), Authors = String.Join(", ", resultParse["authors"].ToObject<string[]>()) };
      // foreach (KeyValuePair<string, JToken> kvp in resultParse.Children())
      // {
      //   if (kvp.Key == "data")
      //   {
      //     newResult = (JArray)kvp.Value;
      //   }
      // }

      // List<Book> bookList = JsonConvert.DeserializeObject<List<Book>>(newResult.ToString());

      return book;
    }
    public static Book GetDetails(int id)
    {
      var apiCallTask = ApiHelper.Get(id);
      var result = apiCallTask.Result;

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