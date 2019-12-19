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
        private IStoreWrapper _dataStore;
        private ILogger<EditModel> _logger;

        [TempData]
        public string Message { get; set; }

        [BindProperty]
        public Data.Models.Water Water { get; set; }

        public EditModel(
            IStoreWrapper dataStore,
            ILogger<EditModel> logger)
        {
            _dataStore = dataStore;
            _logger = logger;
        }

        public async Task OnGetAsync(
            Guid id)
        {
            var waters = await _dataStore.Waters.GetValuesAsync();
            Water = waters.FirstOrDefault(_ => _.Id == id);
        }

        public async Task<ActionResult> OnPostAsync()
        {
            var water = await _dataStore.Waters.UpdateAsync(Water, Water.Id);
            
            if(water == null)
            {
                Message = "Fehler beim ?ndern des Gew?ssers";
                return Page();
            }

            Message = $"Gew?ser '{Water.Longname}' ge?dert.";
            return new RedirectToPageResult("./Index");
        }
    }
}
