using Blog.Data;
using Blog.Models;
using Blog.Models.BlogViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Pages.Articles
{
    public class ArticleTagsPageModel : PageModel
    {
        public List<AssignedTagData> AssignedTagDataList;

        public void PopulateAssignedTagData(BlogContext context,
                                       Article article)
        {
            var allTags = context.Tags.Where(t => !t.Disabled);
            var articleTags = new HashSet<string>(
                article.Tags.Select(t => t.Id));

            AssignedTagDataList = new List<AssignedTagData>();
            foreach (var tag in allTags)
            {
                AssignedTagDataList.Add(new AssignedTagData
                {
                    TagId = tag.Id,
                    Assigned = articleTags.Contains(tag.Id)
                });
            }
        }

    }
}
