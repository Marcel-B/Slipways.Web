using com.b_velop.Slipways.Web.Data.Models;
using com.b_velop.Slipways.Web.Infrastructure;
using com.b_velop.Slipways.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.b_velop.Slipways.Web.Pages
{
    public class SlipwaysModel
    {
        public HashSet<Slipway> Slipways { get; set; }
    }

    public class IndexModel : PageModel
    {
        private readonly IMemoryCache _cache;
        private readonly IGraphQLService _graphQLService;
        private readonly ISecretProvider _secretProvider;
        private readonly ISlipwayService _slipwayService;
        private readonly ILogger<IndexModel> _logger;

        //[BindProperty]
        //public HashSet<Slipway> Slipways { get; set; }

        public SlipwaysModel Slipways { get; set; }

        public IndexModel(
            IMemoryCache cache,
            IGraphQLService graphQLService,
            ISecretProvider secretProvider,
            ISlipwayService slipwayService,
            ILogger<IndexModel> logger)
        {
            _cache = cache;
            _graphQLService = graphQLService;
            _secretProvider = secretProvider;
            _slipwayService = slipwayService;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            Slipways = new SlipwaysModel();
            if (!_cache.TryGetValue("Slipways", out HashSet<Slipway> slipways))
            {
                var slipwayDtos = await _graphQLService.GetSlipwaysAsync();
                slipways = new HashSet<Slipway>();
                foreach (var slipwayDto in slipwayDtos)
                    slipways.Add(new Slipway(slipwayDto));
                _cache.Set("Slipways", slipways);
            }
            Slipways.Slipways = slipways;
        }

        public IActionResult OnGetFilter(
            [FromQuery] string search,
            [FromQuery] bool onlyFree)
        {
            Slipways = new SlipwaysModel();
            if (_cache.TryGetValue("Slipways", out HashSet<Slipway> slipways))
            {
                IEnumerable<Slipway> cs;
                if (onlyFree)
                    cs = slipways.Where(_ => _.Costs <= 0);
                else
                    cs = slipways;
                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.ToLower();
                    cs = cs.Where(_ => _.Name.ToLower().Contains(search) || _.City.ToLower().Contains(search)).Distinct();
                }
                Slipways.Slipways = new HashSet<Slipway>(cs);
            }
            var partial = Partial("_SlipwayTable", Slipways);
            return partial;
        }
    }
}
