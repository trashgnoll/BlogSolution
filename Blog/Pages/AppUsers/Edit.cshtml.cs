using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Blog.Data;
using Blog.Models;
using Blog.Areas.Identity.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Blog.Pages.AppUsers
{
    public class EditModel : PageModel
    {
        private readonly Blog.Data.BlogContext _context;
        UserManager<BlogUser> _userManager;
        Claim adminClaimToEdit;
        Claim editorClaimToEdit;

        public EditModel(UserManager<BlogUser> userManager, Blog.Data.BlogContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public BlogUser AppUser { get; set; } = default!;
        [BindProperty]
        public bool CurrentAdminClaim { get; set; }
        [BindProperty]
        public bool InitialAdminClaim { get; set; }
        [BindProperty]
        public bool CurrentEditorClaim { get; set; }
        [BindProperty]
        public bool InitialEditorClaim { get; set; }


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
            AppUser = user;

            adminClaimToEdit = _userManager.GetClaimsAsync(user).Result.Where(c => c.Type == "IsAdmin").FirstOrDefault();
            InitialAdminClaim = adminClaimToEdit != null ?  true : false;
            CurrentAdminClaim = InitialAdminClaim;

            editorClaimToEdit = _userManager.GetClaimsAsync(user).Result.Where(c => c.Type == "IsEditor").FirstOrDefault();
            InitialEditorClaim = editorClaimToEdit != null ? true : false;
            CurrentEditorClaim = InitialEditorClaim;


            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            var userToUpdate = await _context.BlogUser.FindAsync(AppUser.Id);


            if (userToUpdate == null)
            {
                return NotFound();
            }

            // Uses the posted form values from the PageContext property in the PageModel.
            // Updates only the properties listed(s => s.FirstMidName, s => s.LastName, s => s.EnrollmentDate).
            // Looks for form fields with a "AppUser" prefix.For example, Student.FirstMidName.It's not case sensitive
            if (await TryUpdateModelAsync<BlogUser>(
                userToUpdate,
                "AppUser",
                s => s.Access)) 

            {
                if (CurrentAdminClaim != InitialAdminClaim)
                {
                    if (CurrentAdminClaim == true)
                    {
                        Claim claim = new Claim("IsAdmin", "");
                        await _userManager.AddClaimAsync(userToUpdate, claim);
                    }
                    else
                    {
                        adminClaimToEdit = _userManager.GetClaimsAsync(userToUpdate).Result.Where(c => c.Type == "IsAdmin").FirstOrDefault();
                        await _userManager.RemoveClaimAsync(userToUpdate, adminClaimToEdit);
                    }
                }

                if (CurrentEditorClaim != InitialEditorClaim)
                {
                    if (CurrentEditorClaim == true)
                    {
                        Claim claim = new Claim("IsEditor", "");
                        await _userManager.AddClaimAsync(userToUpdate, claim);
                    }
                    else
                    {
                        editorClaimToEdit = _userManager.GetClaimsAsync(userToUpdate).Result.Where(c => c.Type == "IsEditor").FirstOrDefault();
                        await _userManager.RemoveClaimAsync(userToUpdate, editorClaimToEdit);
                    }
                }


                // _context.Attach(AppUser).State = EntityState.Modified;
                var i = await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        //public async Task<IActionResult> OnPostAsync()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }

        //    _context.Attach(AppUser).State = EntityState.Modified;
        //    //_context.Entry(AppUser).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!AppUserExists(AppUser.Id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return RedirectToPage("./Index");
        //}

        private bool AppUserExists(string id)
        {
            bool res = (_context.BlogUser?.Any(e => e.Id == id)).GetValueOrDefault();
            return (_context.BlogUser?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
