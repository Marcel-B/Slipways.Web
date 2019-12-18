using com.b_velop.Slipways.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace com.b_velop.Slipways.Web.Areas.Admin.Pages.Water
{
    public class CreateModel : PageModel
    {
        [TempData]
        public string Message { get; set; }

        [BindProperty]
        public Data.Models.Water Water { get; set; }

        private IStoreWrapper _dataStore;

        public CreateModel(
            IStoreWrapper dataStore)
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
            var reuslt = await _dataStore.Waters.AddAsync(Water);

            if (reuslt != null)
                return RedirectToPage("./Index");

            Message = "Fehler beim erstellen des Gewässers";
            return Page();
        }
    }
}
