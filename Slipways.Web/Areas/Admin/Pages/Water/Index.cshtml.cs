using com.b_velop.Slipways.Web.Data.Dtos;
using com.b_velop.Slipways.Web.Infrastructure;
using com.b_velop.Slipways.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.b_velop.Slipways.Web.Areas.Admin.Pages.Water
{
    public class IndexModel : PageModel
    {
        private ISlipwayService _slipwayService;

        [BindProperty]
        public HashSet<WaterDto> Waters { get; set; }
        public IndexModel(
            IMemoryCache cache,
            ISecretProvider secretProvider,
            ISlipwayService slipwayService,
            ILogger<IndexModel> logger)
        {
            _slipwayService = slipwayService;
        }

        public async Task OnGetAsync()
        {
            Waters = new HashSet<WaterDto>(await _slipwayService.GetWaterAsync());
        }
    }
}
