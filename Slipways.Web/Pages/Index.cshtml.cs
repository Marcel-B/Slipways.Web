using com.b_velop.Slipways.Web.Data.Models;
using com.b_velop.Slipways.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.b_velop.Slipways.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ISlipwayService _slipwayService;
        private readonly ILogger<IndexModel> _logger;

  

        [BindProperty]
        public IEnumerable<Slipway> Slipways { get; set; }

        public IndexModel(
            ISlipwayService slipwayService,
            ILogger<IndexModel> logger)
        {
            _slipwayService = slipwayService;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            Slipways = await _slipwayService.GetSlipwaysAsync();
        }

        public void OnPost()
        {

        }
    }
}
