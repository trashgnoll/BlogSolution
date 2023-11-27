using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Blog.Data;
using Blog.Models;
using Blog.Models.BlogViewModels;

namespace Blog.Pages.Articles
{
    public class DetailsModel : PageModel
    {
        private readonly BlogContext _context;

        public DetailsModel(BlogContext context)
        {
            _context = context;
        }

        //public ArticleCommentsData ArticleComments { get; set; }


        public Article Article { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Article == null)
            {
                return NotFound();
            }

            var article = await _context.Article.Include(a => a.Comments).FirstOrDefaultAsync(m => m.Id == id) ;
            if (article == null)
            {
                return NotFound();
            }

            Article = article;
            return Page();
        }
    }
}

