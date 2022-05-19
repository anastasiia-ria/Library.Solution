using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels
{
  public class BackgroundViewModel
  {
    [Required(ErrorMessage = "Please choose background image")]
    [Display(Name = "Background")]
    public IFormFile Background { get; set; }

    public int RoomId { get; set; }
  }
}