using System.Collections.Generic;
using System.Threading.Tasks;
using com.b_velop.Slipways.Web.Infrastructure;
using com.b_velop.Slipways.Web.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace com.b_velop.Slipways.Web.Areas.Admin.Pages.Slipways
{

    public class SlipwaysModel
    {
        public HashSet<Data.Models.Slipway> Slipways { get; set; }
    }

    public class IndexModel : PageModel
    {
        private readonly IMemoryCache _cache;
        private readonly ISecretProvider _secretProvider;
        private readonly ISlipwayService _slipwayService;
        private readonly ILogger<IndexModel> _logger;

        //[BindProperty]
        //public HashSet<Slipway> Slipways { get; set; }

        public SlipwaysModel Slipways { get; set; }

        public IndexModel(
            IMemoryCache cache,
            ISecretProvider secretProvider,
            ISlipwayService slipwayService,
            ILogger<IndexModel> logger)
        {
            _cache = cache;
            _secretProvider = secretProvider;
            _slipwayService = slipwayService;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            Slipways = new SlipwaysModel();
            if (!_cache.TryGetValue("Slipways", out HashSet<Data.Models.Slipway> slipways))
            {
                var slipwaysDto = await _slipwayService.GetSlipwaysAsync();
                slipways = new HashSet<Data.Models.Slipway>();
                foreach (var slipwayDto in slipwaysDto)
                {
                    slipways.Add(new Data.Models.Slipway(slipwayDto));
                }
                _cache.Set("Slipways", slipways);
            }
            Slipways.Slipways = slipways;
        }
    }
}