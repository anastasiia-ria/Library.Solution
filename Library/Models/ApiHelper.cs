using System.Threading.Tasks;
using RestSharp;
using System;
namespace Library.Models
{
  class ApiHelper
  {
    public static async Task<string> GetAll(string general, string title, string authors, string publisher, string isbn)
    {
      Console.WriteLine($"volumes?q={general}{title}{authors}{publisher}{isbn}");
      var General = String.IsNullOrEmpty(general) ? "" : general;
      var Title = String.IsNullOrEmpty(title) ? "" : $"+intitle:{title}";
      var Authors = String.IsNullOrEmpty(authors) ? "" : $"+inauthor:{authors}";
      var Publisher = String.IsNullOrEmpty(publisher) ? "" : $"+inpublisher:{publisher}";
      var ISBN = String.IsNullOrEmpty(isbn) ? "" : $"+isbn:{isbn}";
      RestClient client = new RestClient($"https://www.googleapis.com/books/v1/");
      RestRequest request = new RestRequest($"volumes?q={General}{Title}{Authors}{Publisher}{ISBN}&maxResults=8", Method.GET);
      var response = await client.ExecuteTaskAsync(request);
      Console.WriteLine($"volumes?q={General}{Title}{Authors}{Publisher}{ISBN}");
      return response.Content;
    }
    public static async Task<string> Get(string id)
    {
      RestClient client = new RestClient("https://www.googleapis.com/books/v1/volumes/");
      RestRequest request = new RestRequest($"{id}", Method.GET);
      var response = await client.ExecuteTaskAsync(request);
      return response.Content;
    }
    public static async Task Post(string newAnimal)
    {
      RestClient client = new RestClient("http://localhost:5004/api");
      RestRequest request = new RestRequest($"animals", Method.POST);
      request.AddHeader("Content-Type", "application/json");
      request.AddJsonBody(newAnimal);
      var response = await client.ExecuteTaskAsync(request);
    }
    public static async Task Put(int id, string newAnimal)
    {
      RestClient client = new RestClient("http://localhost:5004/api");
      RestRequest request = new RestRequest($"animals/{id}", Method.PUT);
      request.AddHeader("Content-Type", "application/json");
      request.AddJsonBody(newAnimal);
      var response = await client.ExecuteTaskAsync(request);
    }
    public static async Task Delete(int id)
    {
      RestClient client = new RestClient("http://localhost:5004/api");
      RestRequest request = new RestRequest($"animals/{id}", Method.DELETE);
      request.AddHeader("Content-Type", "application/json");
      var response = await client.ExecuteTaskAsync(request);
    }
  }
}