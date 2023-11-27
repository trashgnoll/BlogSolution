using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Blog.Areas.Identity.Data;

public class BlogUser : IdentityUser
{
    public string Access { get; set; }
}

