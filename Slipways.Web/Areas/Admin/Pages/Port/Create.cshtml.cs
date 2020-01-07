using com.b_velop.Slipways.Web.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace com.b_velop.Slipways.Web.Areas.Admin.Pages.Port
{
    public class CreateModel : PageModel
    {
        private IStoreWrapper _dataStore;
        private ILogger<CreateModel> _logger;

        [TempData]
        public string Message { get; set; }

        [BindProperty]
        public Guid WaterId { get; set; }

        public SelectList Waters { get; set; }

        [BindProperty]
        public Guid SlipwayId { get; set; }

        public SelectList Slipways { get; set; }

        [BindProperty]
        public b_velop.Slipways.Data.Models.Port Port { get; set; }

        public CreateModel(
            IStoreWrapper dataStore,
            ILogger<CreateModel> logger)
        {
            _dataStore = dataStore;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            var waters = await _dataStore.Waters.GetValuesAsync();
            Waters = new SelectList(waters, "Id", "Longname");
            var slipways = await _dataStore.Slipways.GetValuesAsync();
            Slipways = new SelectList(slipways, "Id", "Name");
        }

        public void OnPostAsync()
        {

        }
    }
}
