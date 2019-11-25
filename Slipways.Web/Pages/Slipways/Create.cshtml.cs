using com.b_velop.Slipways.Web.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace com.b_velop.Slipways.Web.Pages.Slipways
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public Slipway Slipway { get; set; }
        public void OnGet()
        {
            Slipway = new Slipway();

        }
    }
}