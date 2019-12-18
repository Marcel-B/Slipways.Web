using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.b_velop.Slipways.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace com.b_velop.Slipways.Web.Areas.Admin.Pages.Slipway
{
    public class EditModel : PageModel
    {
        private IStoreWrapper _dataStore;
        private ILogger<EditModel> _logger;

        [TempData]
        public string Message { get; set; }

        [BindProperty]
        public Data.Models.Slipway Slipway { get; set; }

        [BindProperty]
        public string SelectedWaterId { get; set; }

        [BindProperty]
        public string WaterId { get; set; }

        public SelectList Waters { get; set; }

        public int Idx { get; set; }

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
            SelectedWaterId = Slipway.Water.Id.ToString();
            var extras = await _dataStore.Extras.GetValuesAsync();
            Waters = await GetSelectListAsync();
            Slipway.Extras = extras;
        }

        private async Task<SelectList> GetSelectListAsync()
        {
            var waters = await _dataStore.Waters.GetValuesAsync();
            return new SelectList(waters.OrderBy(_ => _.Longname), "Id", "Longname");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var waters = await _dataStore.Waters.GetValuesAsync();
            var water = waters.First(_ => _.Id == Guid.Parse(WaterId));
            Slipway.Water = water;
            var slipways = await _dataStore.Slipways.UpdateAsync(Slipway, Slipway.Id);
            if (slipways == null)
            {
                Message = "Ändern der Slipanlage fehlgeschlagen";
                Waters = await GetSelectListAsync();
                return Page();
            }
            Message = $"Slipanlage '{Slipway.Name}' erfolgreich geändert.";
            return new RedirectToPageResult("./Index");
        }
    }
}
