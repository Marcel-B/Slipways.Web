using System;
using System.Linq;
using System.Threading.Tasks;
using com.b_velop.Slipways.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace com.b_velop.Slipways.Web.Areas.Admin.Pages.Slipway
{
    public class EditModel : PageModel
    {
        private IStoreWrapper _dataStore;
        private ILogger<EditModel> _logger;

        [BindProperty]
        public Data.Models.Slipway Slipway { get; set; }

        public EditModel(
            IStoreWrapper dataStore,
            ILogger<EditModel> logger)
        {
            _dataStore = dataStore;
            _logger = logger;
        }

        public async Task OnGetAsync(
            Guid id)
        {
            var slipways = await _dataStore.Slipways.GetValuesAsync();
            Slipway = slipways.FirstOrDefault(_ => _.Id == id);
            var extras = await _dataStore.Extras.GetValuesAsync();
            Slipway.Extras = extras;
        }
    }
}
