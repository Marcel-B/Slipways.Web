using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.b_velop.Slipways.Web.Data;
using com.b_velop.Slipways.Web.Data.Dtos;
using com.b_velop.Slipways.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace com.b_velop.Slipways.Web.Areas.Admin.Pages.Slipway
{
    public class CreateModel : PageModel
    {
        private IDataStore _dataStore;
        private ILogger<CreateModel> _logger;

        [BindProperty]
        public Data.Models.Slipway Slipway { get; set; }

        [BindProperty]
        public bool ParkingPlace { get; set; }

        [BindProperty]
        public bool Steg { get; set; }

        [BindProperty]
        public bool CampingArea { get; set; }

        public SelectList Waters { get; set; }

        public CreateModel(
            IDataStore dataStore,
            ILogger<CreateModel> logger)
        {
            _dataStore = dataStore;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            Slipway = new Data.Models.Slipway();
            var waters = await _dataStore.GetWatersAsync();
            Waters = new SelectList(waters, "Id", "Longname");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var extras = new List<Guid>();
                if (ParkingPlace)
                    extras.Add(Guid.Parse("8976CEB5-19D6-4F5C-A34D-A43801667B40"));
                if (CampingArea)
                    extras.Add(Guid.Parse("F5836F04-E23B-475A-A079-1E4F3C9C4D87"));
                if (Steg)
                    extras.Add(Guid.Parse("06448FD8-DCC1-4579-947A-8A7B18BC1AAB"));
                var slipways = await _dataStore.AddSlipwayAsync(Slipway, extras);
                if (slipways != null)
                    return RedirectToPage("./Index");
            }
            var waters = await _dataStore.GetWatersAsync();
            Waters = new SelectList(waters, "Id", "Longname");
            return Page();
        }
    }
}