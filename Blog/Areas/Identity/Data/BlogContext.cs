using Blog.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Blog.Models;
using System.Reflection.Emit;

namespace Blog.Data;

public class BlogContext : IdentityDbContext<BlogUser>
{
    public BlogContext(DbContextOptions<BlogContext> options)
        : base(options)
    {
    }

    public DbSet<BlogUser> BlogUser { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.Entity<Article>().ToTable(nameof(Article))
            .HasMany(c => c.Tags)
            .WithMany(i => i.Articles);
        builder.Entity<Tag>().ToTable(nameof(Tag));
        builder.Entity<Comment>().ToTable(nameof(Comment));

    }

    public DbSet<Blog.Models.Article>? Article { get; set; }


}
