using BlogApi.Dto;
using Blog.Data;
using Blog.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogApi.Mapped;

public class Comments
{
    public static async Task<IResult> CreateNewComment(CommentDto comment, IHttpContextAccessor httpContextAccessor, BlogContext db)
    {
        var articleToComment = await db.Articles.FirstOrDefaultAsync(s => s.Id == comment.ArticleId);
        if (articleToComment == null) return Results.NotFound();

        Comment newComment = new();
        newComment.ArticleId = comment.ArticleId;
        newComment.LastEditDate = DateTime.Now;
        newComment.Author = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        newComment.Content = comment.Content;

        db.Comments.Add(newComment);
        await db.SaveChangesAsync();

        return Results.Created($"/Articles/Comments/{newComment.ArticleId}", newComment);

    }

    public static async Task<IResult> DeleteComment(int id, IHttpContextAccessor httpContextAccessor, BlogContext db)
    {
        if (await db.Comments.FindAsync(id) is Comment toDelete)
        {
            if (!IsAuthorOrEditor(toDelete, httpContextAccessor)) { return Results.Unauthorized(); } 

            db.Comments.Remove(toDelete);
            await db.SaveChangesAsync();
            return Results.Ok(toDelete);
        }
        return Results.NotFound();
    }

    private static bool IsAuthorOrEditor(Post item, IHttpContextAccessor httpContextAccessor)
    {
        var isAuthor = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value == item.Author;
        var isEditor = httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == "IsEditor").FirstOrDefault() != null;
        return isAuthor || isEditor;
    }

}
