using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Factory.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Factory.ViewModels;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Factory.Controllers
{
    public class HomeController : Controller
    {
        private readonly FactoryContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(UserManager<ApplicationUser> userManager, FactoryContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        [HttpGet("/")]
        public async Task<ActionResult> Index()
        {
            Engineer[] engineers = _db.Engineers.ToArray();
            Dictionary<string, object[]> model = new Dictionary<string, object[]>();
            model.Add("engineers", engineers);

            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
            if (currentUser != null)
            {
                Machine[] machines = _db.Machines.Where(entry => entry.User.Id == currentUser.Id).ToArray();
                model.Add("machines", machines);
            }
            return View(model);
        }

    }
}