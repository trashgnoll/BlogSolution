using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Blog.Data;
using Blog.Models;
using Blog.Areas.Identity.Data;

namespace Blog.Pages.AppUsers
{
    public class DeleteModel : PageModel
    {
        private readonly Blog.Data.BlogContext _context;

        public DeleteModel(Blog.Data.BlogContext context)
        {
            _context = context;
        }
        [BindProperty]
        public BlogUser AppUser { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.BlogUser == null)
            {
                return NotFound();
            }

            var user = await _context.BlogUser.FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                AppUser = user;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null || _context.BlogUser == null)
            {
                return NotFound();
            }
            var user = await _context.BlogUser.FindAsync(id);

            if (user != null)
            {
                AppUser = user;
                _context.BlogUser.Remove(AppUser);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

    }
}
