using com.b_velop.Slipways.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace com.b_velop.Slipways.Web.Areas.Admin.Pages.Water
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public Data.Models.Water Water { get; set; }

        private IDataStore _dataStore;

        public CreateModel(
            IDataStore dataStore)
        {
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
            _ = await _dataStore.AddWaterAsync(Water);
            return RedirectToPage("./Index");
        }
    }
}
