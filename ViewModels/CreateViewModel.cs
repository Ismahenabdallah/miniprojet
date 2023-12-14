using atelier3.Models;
using System.ComponentModel.DataAnnotations;
namespace atelier3.ViewModels
{
    public class CreateViewModel
    {

        public int ProductID { get; set; }
        public Categories Category { get; set; }
        public double Price { get; set; }
        public Size ProductSize { get; set; }
        public IFormFile Photo { get; set; }
    }

}
