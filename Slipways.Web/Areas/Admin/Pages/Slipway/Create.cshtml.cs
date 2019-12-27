using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.b_velop.Slipways.Web.Data;
using com.b_velop.Slipways.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace com.b_velop.Slipways.Web.Areas.Admin.Pages.Slipway
{
    public class CreateModel : PageModel
    {
        public class ExtraSelection
        {
            public Guid Id { get; set; }
            public bool Selected { get; set; }
        }

        private IStoreWrapper _dataStore;
        private ILogger<CreateModel> _logger;

        [TempData]
        public string Message { get; set; }

        [BindProperty]
        public Dictionary<string, ExtraSelection> Extras { get; set; }

        [BindProperty]
        public b_velop.Slipways.Data.Models.Slipway Slipway { get; set; }

        [BindProperty]
        public bool ParkingPlace { get; set; }

        [BindProperty]
        public bool Steg { get; set; }

        [BindProperty]
        public bool CampingArea { get; set; }

        [BindProperty]
        public Guid WaterId { get; set; }

        public SelectList Waters { get; set; }

        public CreateModel(
            IStoreWrapper dataStore,
            ILogger<CreateModel> logger)
        {
            _dataStore = dataStore;
            _logger = logger;
            Extras = new Dictionary<string, ExtraSelection>();
        }

        public async Task OnGetAsync()
        {
            Slipway = new com.b_velop.Slipways.Data.Models.Slipway();
            var waters = await _dataStore.Waters.GetValuesAsync();
            Waters = new SelectList(waters, "Id", "Longname");
            var extras = await _dataStore.Extras.GetValuesAsync();
            foreach (var extra in extras)
                Extras[extra.Name] = new ExtraSelection { Id = extra.Id, Selected = false };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var waters = await _dataStore.Waters.GetValuesAsync();
            var water = waters.First(_ => _.Id == WaterId);
            Slipway.Water = water;
            Slipway.WaterFk = WaterId;
            if (ModelState.IsValid)
            {
                if (ParkingPlace)
                    Slipway.Extras.Add(new Extra { Id = Guid.Parse("8976CEB5-19D6-4F5C-A34D-A43801667B40") });
                if (CampingArea)
                    Slipway.Extras.Add(new Extra { Id = Guid.Parse("F5836F04-E23B-475A-A079-1E4F3C9C4D87") });
                if (Steg)
                    Slipway.Extras.Add(new Extra { Id = Guid.Parse("06448FD8-DCC1-4579-947A-8A7B18BC1AAB") });


                var slipways = await _dataStore.Slipways.AddAsync(Slipway);
                if (slipways != null)
                    return RedirectToPage("./Index");

                Message = "Fehler beim erstellen der Slipanlage";
            }
            else
            {
                Message = "Eingabe ungültig";
            }

            Waters = new SelectList(waters, "Id", "Longname");

            return Page();
        }
    }
}