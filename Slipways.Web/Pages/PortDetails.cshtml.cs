using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.b_velop.Slipways.Web.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace com.b_velop.Slipways.Web
{
    public class PortDetailsModel : PageModel
    {
        private IStoreWrapper _dataStore;
        private ILogger<PortDetailsModel> _logger;

        [BindProperty]
        public Slipways.Data.Models.Port Port { get; set; }

        public PortDetailsModel(
            IStoreWrapper dataStore,
            ILogger<PortDetailsModel> logger)
        {
            _dataStore = dataStore;
            _logger = logger;
        }

        public async Task OnGetAsync(
            Guid id)
        {
            var ports = await _dataStore.Ports.GetValuesAsync();
            var port = ports.First(_ => _.Id == id);
            Port = port;
        }
    }
}