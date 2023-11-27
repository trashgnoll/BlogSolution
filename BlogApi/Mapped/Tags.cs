using BlogApi.Dto;
using Blog.Data;
using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Mapped;

public static class Tags
{
    public static async Task<List<Tag>> TagIndex(BlogContext db)
    {
        return await db.Tags.AsNoTracking().ToListAsync();
    }

    public static async Task<IResult> CreateNewTag(TagDto tag, BlogContext db)
    {
        Tag newTag = new()
        {
            Id = tag.Id,
            Disabled = false
        };
        db.Tags.Add(newTag);
        await db.SaveChangesAsync();
        return Results.Created($"/Tags/{newTag.Id}", newTag);
    }

    public static async Task<IResult> DeleteTag(TagDto tagToDelete, BlogContext db)
    {
        if (await db.Tags.FindAsync(tagToDelete.Id) is Tag toDelete)
        {
            db.Tags.Remove(toDelete);
            await db.SaveChangesAsync();
            return Results.Ok(toDelete);
        }
        return Results.NotFound();
    }
}
