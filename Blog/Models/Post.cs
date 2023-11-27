using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Linq;

namespace Blog.Models
{
    public abstract class Post
    {
        public int Id { get; set; }
        public string Author { get; set; }
        [Display(Name = "Last Edited")]
        public DateTime LastEditDate { get; set; }
        public String Content { get; set; }
    }
}
