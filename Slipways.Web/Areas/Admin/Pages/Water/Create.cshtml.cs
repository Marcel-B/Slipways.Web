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
        private IGraphQLService _graphQLService;
        private IMemoryCache _cache;
        private IWaterService _service;

        public CreateModel(
            WaterViewModel viewModel,
            IDataStore dataStore,
            IGraphQLService graphQLService,
            IMemoryCache cache,
            IWaterService service)
        {
            ViewModel = viewModel;
            _dataStore = dataStore;
            _graphQLService = graphQLService;
            _cache = cache;
            _service = service;
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
            var water = await ViewModel.SaveWaterAsync(_service);
            if (!_cache.TryGetValue("waters", out HashSet<Data.Models.Water> waters))
            {
                var waterDtos = await _graphQLService.GetWatersAsync();
                waters = new HashSet<Data.Models.Water>();
                foreach (var waterDto in waterDtos)
                {
                    waters.Add(new Data.Models.Water
                    {
                        Id = waterDto.Id,
                        Longname = waterDto.Longname,
                        Shortname = waterDto.Shortname
                    });
                }
                _cache.Set("waters", waters);
            }
            else
            {
                waters.Add(new Data.Models.Water
                {
                    Id = water.Id,
                    Longname = water.Longname,
                    Shortname = water.Shortname
                });
                _cache.Set("waters", waters);
            }
            return RedirectToPage("./Index");
        }
    }
}
