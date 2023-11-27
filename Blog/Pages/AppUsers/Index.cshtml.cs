using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Blog.Areas.Identity.Data;
using Blog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Blog.Pages.AppUsers
{
    public class DisplayItem
    {
        public BlogUser UserItem { get; set; }
        public bool HasAdminClaim { get; set; }
        public bool HasEditorClaim { get; set; }
        public DisplayItem(BlogUser userItem, bool hasAdminClaim, bool hasEditorClaim)
        {
            UserItem = userItem;
            HasAdminClaim = hasAdminClaim;
            HasEditorClaim = hasEditorClaim;
        }
    }
    [Authorize("CanManageUsers")]
    public class IndexModel : PageModel
    {
        public IList<BlogUser> AppUser { get; set; } = default!; //
        public IList<DisplayItem> Users { get; set; } //

        readonly UserManager<BlogUser> _userManager;
        private readonly Blog.Data.BlogContext _context;

        public IndexModel(UserManager<BlogUser> userManager,
            Blog.Data.BlogContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task OnGetAsync()
        {
            if (_context.BlogUser != null)
            {
                //AppUser = await _context.BlogUser.ToListAsync();
                AppUser = await _context.BlogUser.ToListAsync();
                Users = new List<DisplayItem>();

                foreach (var u in AppUser)
                {
                    var adminClaim = _userManager.GetClaimsAsync(u).Result.Where(c => c.Type == "IsAdmin").FirstOrDefault();
                    bool hasAdminClaim = adminClaim != null;
                    var editorClaim = _userManager.GetClaimsAsync(u).Result.Where(c => c.Type == "IsEditor").FirstOrDefault();
                    bool hasEditorClaim = editorClaim != null;

                    Users.Add(new DisplayItem(u, hasAdminClaim, hasEditorClaim));
                }
            }
        }
    }
}

