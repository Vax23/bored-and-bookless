using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BoredAndBookless.Models;

namespace BoredAndBookless.Pages
{
    public class OdjavaModel : PageModel
    {
        public IActionResult OnPost()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            else
            {
                SessionClass.SessionId = null;
                SessionClass.TipKorisnika = null;

                return RedirectToPage("./Index");
            }
        }
    }
}
