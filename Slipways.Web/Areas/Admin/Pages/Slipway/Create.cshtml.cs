using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private ISlipwayService _service;
        private IMemoryCache _cache;
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
            ISlipwayService service,
            IMemoryCache cache,
            ILogger<CreateModel> logger)
        {
            _service = service;
            _cache = cache;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            Slipway = new Data.Models.Slipway();
            if (!_cache.TryGetValue("waters", out IEnumerable<Data.Models.Water> waters))
            {
                var result = await _service.GetWatersAsync();
                _cache.Set("waters", result);
                waters = result;
            }
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

                var slipwayDto = new SlipwayDto
                {
                    Name = Slipway.Name,
                    City = Slipway.City,
                    Latitude = Slipway.Latitude.Value,
                    Longitude = Slipway.Longitude.Value,
                    Costs = Slipway.Costs.Value,
                    Rating = Slipway.Rating.Value,
                    Street = Slipway.Street,
                    Postalcode = Slipway.Postalcode,
                    WaterFk = Guid.Parse(Slipway.Water),
                    Pro = Slipway.Pro,
                    Contra = Slipway.Contra,
                    Comment = Slipway.Comment,
                    Created = DateTime.Now,
                    Extras = extras
                };
                var result = await _service.InsertSlipway(slipwayDto);
                if (result != null)
                {
                    return RedirectToPage("../Index");
                }
            }
            if (!_cache.TryGetValue("waters", out IEnumerable<Data.Models.Water> waters))
            {
                var result = await _service.GetWatersAsync();
                _cache.Set("waters", result);
                waters = result;
            }
            Waters = new SelectList(waters, "Id", "Longname");
            return Page();
        }
    }
}