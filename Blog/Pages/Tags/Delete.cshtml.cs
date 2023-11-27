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
    public class DeleteModel : PageModel
    {
        private readonly Blog.Data.BlogContext _context;

        public DeleteModel(Blog.Data.BlogContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Tag Tag { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.Tags == null)
            {
                return NotFound();
            }

            var tag = await _context.Tags.FirstOrDefaultAsync(m => m.Id == id);
            // var tag = await _context.Articles.FirstOrDefaultAsync(m => m.Id == id);

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

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null || _context.Tags == null)
            {
                return NotFound();
            }
            var tag = await _context.Tags.FindAsync(id);

            if (tag != null)
            {
                tag.Disabled = true;

                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
