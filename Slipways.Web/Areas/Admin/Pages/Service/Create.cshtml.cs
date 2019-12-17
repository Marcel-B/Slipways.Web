using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.b_velop.Slipways.Web.Data;
using com.b_velop.Slipways.Web.Data.Dtos;
using com.b_velop.Slipways.Web.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace com.b_velop.Slipways.Web.Areas.Admin.Pages.Service
{
    public class CreateModel : PageModel
    {
        public class ManufacturerSelection
        {
            public Guid Id { get; set; }
            public bool Selected { get; set; }
        }

        private IDataStore _dataStore;
        private ILogger<CreateModel> _logger;

        [BindProperty]
        public Data.Models.Service Service { get; set; }

        [BindProperty]
        public Dictionary<string, ManufacturerSelection> Manufacturers { get; set; }

        public CreateModel(
            IDataStore dataStore,
            ILogger<CreateModel> logger)
        {
            _dataStore = dataStore;
            _logger = logger;
            Manufacturers = new Dictionary<string, ManufacturerSelection>();
        }

        public async Task OnGetAsync()
        {
            var manufacturers = await _dataStore.GetManufacturersAsync();
            if(manufacturers != null)
            {
                foreach (var manufacturer in manufacturers)
                {
                    Manufacturers[manufacturer.Name] = new ManufacturerSelection { Id = manufacturer.Id, Selected = false };
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                foreach (var key in Manufacturers.Keys)
                {
                    if (Manufacturers[key].Selected)
                    {
                        var id = Manufacturers[key].Id;
                        Service.Manufacturers.Add(new Manufacturer { Id = id, Name = key });
                    }
                }
                await _dataStore.AddServiceAsync(Service);
                return new RedirectToPageResult("./");
            }
            return Page();
        }
    }
}
