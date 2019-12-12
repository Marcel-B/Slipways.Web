using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;

namespace com.b_velop.Slipways.Web.Areas.Admin.Pages.Slipway
{
    public class DetailsModel : PageModel
    {
        private IMemoryCache _cache;

        [BindProperty]
        public Data.Models.Slipway Slipway { get; set; }

        public DetailsModel(
            IMemoryCache cache)
        {
            _cache = cache;
        }
        public ActionResult OnGet(
            Guid id)
        {
            if (!_cache.TryGetValue("Slipways", out HashSet<Data.Models.Slipway> slipways))
                return RedirectToPage("./Index");

            Slipway = slipways.FirstOrDefault(_ => _.Id == id);
            return Page();
        }
    }
}
