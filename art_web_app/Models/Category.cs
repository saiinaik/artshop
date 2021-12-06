using System.ComponentModel.DataAnnotations;

namespace art_web_app.Models
{
    public class Category
    {
        [Key] public int Id { get; set; }

        [Display(Name ="Category Name")]
        [Required] 
        public string Name { get; set; }
    }
}
