namespace Blog.Models
{
    public class Article : Post
    {
        public string Title { get; set; }
        public IList<Comment>? Comments { get; set; }
        public ICollection<Tag>? Tags { get; set; }

    }
}
