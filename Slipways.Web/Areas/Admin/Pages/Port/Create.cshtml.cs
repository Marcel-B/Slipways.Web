using com.b_velop.Slipways.Web.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace com.b_velop.Slipways.Web.Areas.Admin.Pages.Port
{
    public class CreateModel : PageModel
    {
        private IStoreWrapper _dataStore;
        private ILogger<CreateModel> _logger;

        [BindProperty]
        public b_velop.Slipways.Data.Models.Port Port { get; set; }

        [TempData]
        public string Message { get; set; }

        public CreateModel(
            IStoreWrapper dataStore,
            ILogger<CreateModel> logger)
        {
            _dataStore = dataStore;
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public void OnPostAsync()
        {

        }
    }
}
