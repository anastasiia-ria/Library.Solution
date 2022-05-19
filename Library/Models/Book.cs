using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
namespace Library.Models
{
  public class Book
  {
    public int BookId { get; set; }
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public string Authors { get; set; }
    public string Publisher { get; set; }
    public string PublishedDate { get; set; }
    public string Description { get; set; }
    public string ISBN_10 { get; set; }
    public string ISBN_13 { get; set; }
    public string ImgID { get; set; }
    public string PageCount { get; set; }
    public string Status { get; set; }
    public string Categories { get; set; }
    public string Language { get; set; }
    public string Rating { get; set; }
    public virtual Shelf Shelf { get; set; }
    public virtual Room Room { get; set; }
    public virtual ApplicationUser User { get; set; }
    public static object[] GetBooks(string general, string title, string authors, string publisher, string isbn, int startIndex)
    {
      var apiCallTask = ApiHelper.GetAll(general, title, authors, publisher, isbn, startIndex);
      var result = apiCallTask.Result;
      var resultParse = JObject.Parse(result)["items"];
      List<Book> bookList = new List<Book>();
      foreach (var item in resultParse)
      {
        JObject info = item["volumeInfo"].ToObject<JObject>();
        var Title = info.ContainsKey("title") ? info["title"].ToString() : "";
        var Authors = info.ContainsKey("authors") ? String.Join(", ", info["authors"].ToObject<string[]>()) : "";
        var Rating = info.ContainsKey("averageRating") ? info["averageRating"].ToString() : "";
        bookList.Add(new Book { Title = Title, Authors = Authors, Rating = Rating, ImgID = item["id"].ToString() });
      }
      object[] array = { bookList, JObject.Parse(result)["totalItems"].ToString() };
      return array;
    }
    public static Book GetDetails(string id)
    {
      var apiCallTask = ApiHelper.Get(id);
      var result = apiCallTask.Result;
      var resultParse = JValue.Parse(result);
      var info = resultParse["volumeInfo"].ToObject<JObject>();
      var Title = info.ContainsKey("title") ? info["title"].ToString() : "";
      var Subtitle = info.ContainsKey("subtitle") ? info["subtitle"].ToString() : "";
      var LanguageCode = info.ContainsKey("language") ? info["language"].ToString() : "";
      var Language = new CultureInfo(LanguageCode).DisplayName;
      var Authors = info.ContainsKey("authors") ? String.Join(", ", info["authors"].ToObject<string[]>()) : "";
      var Categories = info.ContainsKey("categories") ? String.Join(", ", info["categories"].ToObject<string[]>()) : "";
      var Publisher = info.ContainsKey("publisher") ? info["publisher"].ToString() : "";
      var PublishedDate = info.ContainsKey("publishedDate") ? info["publishedDate"].ToString() : "";
      var Description = info.ContainsKey("description") ? info["description"].ToString() : "";
      var PageCount = info.ContainsKey("pageCount") ? info["pageCount"].ToString() : "";
      var Rating = info.ContainsKey("averageRating") ? info["averageRating"].ToString() : "";
      var Identifiers = info.ContainsKey("industryIdentifiers") ? info["industryIdentifiers"] : new JArray();
      var ISBN_10 = "";
      var ISBN_13 = "";
      var first = Identifiers[0].ToObject<JObject>();
      if (first.ContainsKey("type") && first["type"].ToString() == "ISBN_10")
      {
        ISBN_10 = first.ContainsKey("identifier") ? Identifiers[0]["identifier"].ToString() : "";
        ISBN_13 = Identifiers[1].ToObject<JObject>().ContainsKey("identifier") ? Identifiers[1]["identifier"].ToString() : "";
      }
      else
      {
        ISBN_13 = first.ContainsKey("identifier") ? Identifiers[0]["identifier"].ToString() : "";
        ISBN_10 = Identifiers[1].ToObject<JObject>().ContainsKey("identifier") ? Identifiers[1]["identifier"].ToString() : "";
      }
      Book book = new Book { Title = Title, Subtitle = Subtitle, Authors = Authors, Publisher = Publisher, PublishedDate = PublishedDate, Language = Language, Categories = Categories, Description = Description, ISBN_10 = ISBN_10, ISBN_13 = ISBN_13, PageCount = PageCount, ImgID = resultParse["id"].ToString(), Rating = Rating };
      return book;
    }
  }
}