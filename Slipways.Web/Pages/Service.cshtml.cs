using com.b_velop.Slipways.Web.Data;
using com.b_velop.Slipways.Web.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.b_velop.Slipways.Web
{
    public class ServiceModel : PageModel
    {
        private IDataStore _dataStore;
        private ILogger<ServiceModel> _logger;

        [BindProperty]
        public HashSet<Service> Services { get; set; }

        public ServiceModel(
            IDataStore dataStore,
            ILogger<ServiceModel> logger)
        {
            _dataStore = dataStore;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            Services = await _dataStore.GetServicesAsync();
        }
    }
}