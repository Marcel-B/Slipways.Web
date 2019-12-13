using com.b_velop.Slipways.Web.Data;
using com.b_velop.Slipways.Web.Services;
using com.b_velop.Slipways.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.b_velop.Slipways.Web.Areas.Admin.Pages.Water
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public WaterViewModel ViewModel { get; set; }

        private IDataStore _dataStore;

        public CreateModel(
            WaterViewModel viewModel,
            IDataStore dataStore)
        {
            ViewModel = viewModel;
            _dataStore = dataStore;
        }

        public void OnGet()
        {
        }

        public async Task<ActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            ViewModel.Waters = await _dataStore.AddWaterAsync(ViewModel.Water);
            return RedirectToPage("./Index");
        }
    }
}
