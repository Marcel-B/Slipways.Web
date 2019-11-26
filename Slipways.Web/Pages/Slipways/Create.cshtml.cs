using System.Collections.Generic;
using System.Threading.Tasks;
using com.b_velop.Slipways.Web.Data.Models;
using com.b_velop.Slipways.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace com.b_velop.Slipways.Web.Pages.Slipways
{
    public class CreateModel : PageModel
    {
        private ISlipwayService _service;
        private ILogger<CreateModel> _logger;

        [BindProperty]
        public Slipway Slipway { get; set; }

        [BindProperty]
        public IEnumerable<Water> Waters { get; set; }

        public void OnGet(
            ISlipwayService service,
            ILogger<CreateModel> logger)
        {
            _service = service;
            _logger = logger;
            Slipway = new Slipway();
        }

        public async Task GetAsync()
        {
              Waters = await _service.GetWatersAsync();
        }
    }
}