using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Blog.Models
{
    public class Tag
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Tag")]
        [StringLength(20)]
        public string Id { get; set; }
        public bool Disabled { get; set; }

        [System.Text.Json.Serialization.JsonIgnore] 
        public ICollection<Article>? Articles { get; set; }
    }
}
