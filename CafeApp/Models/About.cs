using System.ComponentModel.DataAnnotations;

namespace CafeApp.Models
{
    public class About
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
