using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.b_velop.Slipways.Data.Extensions;
using com.b_velop.Slipways.Data.Helper;
using com.b_velop.Slipways.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace com.b_velop.Slipways.Web.Pages
{
    public class WatersModel : PageModel
    {
        private IDistributedCache _cache;
        private ILogger<WatersModel> _logger;

        [BindProperty]
        public HashSet<Water> Waters { get; set; }

        public WatersModel(
            IDistributedCache cache,
            ILogger<WatersModel> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            var watersBytes = await _cache.GetAsync(Cache.Waters);
            var waters = watersBytes.ToObject<IEnumerable<Water>>();
            Waters = waters.ToHashSet();
        }
    }
}
