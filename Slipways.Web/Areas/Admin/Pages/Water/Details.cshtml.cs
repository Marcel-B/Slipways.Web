using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;

namespace com.b_velop.Slipways.Web.Areas.Admin.Pages.Water
{
    public class DetailsModel : PageModel
    {
        private IMemoryCache _cache;

        [BindProperty]
        public Data.Models.Water Water { get; set; }

        public DetailsModel(
            IMemoryCache cache)
        {
            _cache = cache;
        }

        public void OnGet(
            Guid id)
        {
            if (_cache.TryGetValue("waters", out HashSet<Data.Models.Water> waters))
            {
                Water = waters.FirstOrDefault(_ => _.Id == id);
            }
        }
    }
}
