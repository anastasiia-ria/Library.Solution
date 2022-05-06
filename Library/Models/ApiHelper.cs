using System.Threading.Tasks;
using RestSharp;
using System;
namespace Library.Models
{
  class ApiHelper
  {
    public static async Task<string> GetAll(string isbn)
    {
      var key = EnvironmentVariables.ApiKey;
      RestClient client = new RestClient($"https://www.googleapis.com/books/v1/");
      RestRequest request = new RestRequest($"volumes?key={key}&q=isbn:{isbn}", Method.GET);

      Console.WriteLine($"https://www.googleapis.com/books/v1/key={key}&q=isbn:{isbn}");
      var response = await client.ExecuteTaskAsync(request);
      return response.Content;
    }
    public static async Task<string> Get(int id)
    {
      RestClient client = new RestClient("http://localhost:5004/api");
      RestRequest request = new RestRequest($"animals/{id}", Method.GET);
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