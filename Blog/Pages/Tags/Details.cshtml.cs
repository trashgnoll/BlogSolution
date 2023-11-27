using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Blog.Data;
using Blog.Models;

namespace Blog.Pages.Tags
{
    public class DetailsModel : PageModel
    {
        private readonly Blog.Data.BlogContext _context;

        public DetailsModel(Blog.Data.BlogContext context)
        {
            _context = context;
        }

      public Tag Tag { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.Articles == null)
            {
                return NotFound();
            }

            var tag = await _context.Tags.FirstOrDefaultAsync(m => m.Id == id);
            if (tag == null)
            {
                return NotFound();
            }
            else 
            {
                Tag = tag;
            }
            return Page();
        }
    }
}
