using com.b_velop.Slipways.Web.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Slipways.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public Slipway Slipway { get; set; }

        public IndexModel(
            ILogger<IndexModel> logger)
        {
            _logger = logger;
            Slipway = new Slipway();
        }

        public void OnGet()
        {

        }

        public void OnPost()
        {

        }
    }
}
