using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace com.b_velop.Slipways.Web.Areas.Admin.Pages.Service
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public Data.Models.Service Service { get; set; }

        public void OnGet()
        {
        }
    }
}
