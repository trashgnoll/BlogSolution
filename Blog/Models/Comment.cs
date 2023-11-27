namespace Blog.Models
{
    public class Comment :Post
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
    }
}
