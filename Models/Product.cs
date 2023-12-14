using System.ComponentModel.DataAnnotations.Schema;

namespace atelier3.Models
{



    public enum Categories
    {
        women,
        men,
       
       
    }
    public enum Size
    {
        Small,
        Medium,
        Large,

    }

    public class Product
    {


        public int ProductID { get; set; }
        public Categories Category { get; set; }
        public double Price { get; set; }
        public Size ProductSize { get; set; }
       
        public string PhotoPath { get; set; }

    }
}
