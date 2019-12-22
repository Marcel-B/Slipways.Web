using System.Collections.Generic;
using System.Threading.Tasks;
using com.b_velop.Slipways.Data.Models;
using com.b_velop.Slipways.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace com.b_velop.Slipways.Web.Pages.Slipways
{
    [IgnoreAntiforgeryToken(Order = 2000)]
    public class CreateModel : PageModel
    {
        private IStoreWrapper _dataStore;
        private ILogger<CreateModel> _logger;

        [BindProperty]
        public Slipway Slipway { get; set; }

        public SelectList Waters { get; set; }

        public CreateModel(
            IStoreWrapper dataStore,
            ILogger<CreateModel> logger)
        {
            _dataStore = dataStore;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            Slipway = new Slipway();
            var waterDtos = await _dataStore.Waters.GetValuesAsync();
            var waters = new HashSet<Water>();
            foreach (var waterDto in waterDtos)
            {
                waters.Add(new Water
                {
                    Id = waterDto.Id,
                    Longname = waterDto.Longname,
                    Shortname = waterDto.Shortname
                });
            }
            Waters = new SelectList(waters, "Id", "Longname");
        }
    }
}