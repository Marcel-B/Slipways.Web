using System.Threading.Tasks;
using com.b_velop.Slipways.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace com.b_velop.Slipways.Web.Areas.Admin.Pages.Service
{
    public class CreateModel : PageModel
    {
        private IDataStore _dataStore;
        private ILogger<CreateModel> _logger;

        [BindProperty]
        public Data.Models.Service Service { get; set; }

        public CreateModel(
            IDataStore dataStore,
            ILogger<CreateModel> logger)
        {
            _dataStore = dataStore;
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                await _dataStore.AddServiceAsync(Service);
                return new RedirectToPageResult("./");
            }
            return Page();
        }
    }
}
