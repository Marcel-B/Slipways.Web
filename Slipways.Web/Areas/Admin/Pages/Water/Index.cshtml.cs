using com.b_velop.Slipways.Web.Data.Dtos;
using com.b_velop.Slipways.Web.Infrastructure;
using com.b_velop.Slipways.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.b_velop.Slipways.Web.Areas.Admin.Pages.Water
{
    public class IndexModel : PageModel
    {
        private IMemoryCache _cache;
        private IGraphQLService _graphQLService;
        private IWaterService _waterService;

        [TempData]
        public string Message { get; set; }

        [BindProperty]
        public HashSet<Data.Models.Water> Waters { get; set; }

        public IndexModel(
            IMemoryCache cache,
            IGraphQLService graphQLService,
            ISecretProvider secretProvider,
            IWaterService waterService,
            ILogger<IndexModel> logger)
        {
            _cache = cache;
            _graphQLService = graphQLService;
            _waterService = waterService;
        }

        public async Task OnGetAsync(
            [FromQuery] string id)
        {
            var uuid = Guid.Empty;
            if (id != null)
            {
                uuid = Guid.Parse(id);
            }

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
            if (id != null)
            {
                waters.RemoveWhere(_ => _.Id == uuid);
                var water = await _waterService.DeleteWaterAsync(uuid);
                Message = $"Gewässer '{water?.Longname}' gelöscht";
                _cache.Set("waters", waters);
            }
            Waters = waters;
        }
    }
}
