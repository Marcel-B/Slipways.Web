using System.Collections.Generic;
using System.Threading.Tasks;
using com.b_velop.Slipways.Web.Data.Models;
using com.b_velop.Slipways.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace com.b_velop.Slipways.Web.Pages.Slipways
{
    [IgnoreAntiforgeryToken(Order = 2000)]
    public class CreateModel : PageModel
    {
        private ISlipwayService _service;
        private IGraphQLService _graphQLService;
        private IMemoryCache _cache;
        private ILogger<CreateModel> _logger;

        [BindProperty]
        public Slipway Slipway { get; set; }

        public SelectList Waters { get; set; }

        public CreateModel(
            ISlipwayService service,
            IGraphQLService graphQLService,
            IMemoryCache cache,
            ILogger<CreateModel> logger)
        {
            _service = service;
            _graphQLService = graphQLService;
            _cache = cache;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            Slipway = new Slipway();
            if (!_cache.TryGetValue("waters", out HashSet<Water> waters))
            {
                var waterDtos = await _graphQLService.GetWatersAsync();
                waters = new HashSet<Water>();
                foreach (var waterDto in waterDtos)
                {
                    waters.Add(new Water
                    {
                        Id = waterDto.Id,
                        Longname = waterDto.Longname,
                        Shortname = waterDto.Shortname
                    });
                }
                _cache.Set("waters", waters);
            }
            Waters = new SelectList(waters, "Id", "Longname");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //if (ModelState.IsValid)
            //{
            //    var result = await _service.InsertSlipway(Slipway);
            //    if (result)
            //    {
            //        return RedirectToPage("../Index");
            //    }
            //}
            //if (!_cache.TryGetValue("waters", out IEnumerable<Water> waters))
            //{
            //    var result = await _service.GetWatersAsync();
            //    _cache.Set("waters", result);
            //    waters = result;
            //}
            //Waters = new SelectList(waters, "Id", "Longname");
            return Page();
        }
    }
}