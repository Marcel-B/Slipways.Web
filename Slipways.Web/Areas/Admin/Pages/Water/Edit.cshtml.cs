using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.b_velop.Slipways.Web.Data;
using com.b_velop.Slipways.Web.Data.Dtos;
using com.b_velop.Slipways.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace com.b_velop.Slipways.Web.Areas.Admin.Pages.Water
{
    public class EditModel : PageModel
    {
        private IMemoryCache _cache;
        private IWaterService _waterService;
        private ILogger<EditModel> _logger;

        [TempData]
        public string Message { get; set; }

        [BindProperty]
        public Data.Models.Water Water { get; set; }

        public EditModel(
            IMemoryCache cache,
            IWaterService waterService,
            ILogger<EditModel> logger)
        {
            _cache = cache;
            _waterService = waterService;
            _logger = logger;
        }

        public void OnGet(
            Guid id)
        {
            if (_cache.TryGetValue(Cache.Waters, out HashSet<Data.Models.Water> waters))
            {
                Water = waters.FirstOrDefault(_ => _.Id == id);
            }
        }

        public async Task<ActionResult> OnPostAsync()
        {
            var water = await _waterService.UpdateWaterAsync(new WaterDto
            {
                Id = Water.Id,
                Longname = Water.Longname,
                Shortname = Water.Shortname
            });

            Message = $"Gew�sser '{water.Longname}' ge�ndert.";

            if (water != null && _cache.TryGetValue(Cache.Waters, out HashSet<Data.Models.Water> waters))
            {
                var tmp = waters.RemoveWhere(_ => _.Id == water.Id);
                waters.Add(new Data.Models.Water
                {
                    Id = water.Id,
                    Longname = water.Longname,
                    Shortname = water.Shortname
                });

                _cache.Set(Cache.Waters, waters);
            }
            return new RedirectToPageResult("./Index");
        }
    }
}