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
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Blog.Pages.AppUsers
{
    public class DetailsModel : PageModel
    {
        public IList<Claim> Claims;
        SignInManager<BlogUser> _signInManager;
        UserManager<BlogUser> _userManager;

        private readonly Blog.Data.BlogContext _context;
        public DetailsModel(UserManager<BlogUser> userManager,
            SignInManager<BlogUser> signInManager, Blog.Data.BlogContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public BlogUser AppUser { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.BlogUser == null)
            {
                return NotFound();
            }

            var user = await _context.BlogUser.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                AppUser = user;
                //var u = _userManager.Users.Include(u => u.IsAdmin);
                //var claim = new Claim("Test", DateTime.Now.ToString());
                //await _userManager.AddClaimAsync(AppUser, claim);

                Claims = _userManager.GetClaimsAsync(user).Result;

            }
            return Page();

        }
    }
}
