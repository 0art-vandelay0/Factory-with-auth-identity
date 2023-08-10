using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Factory.Models;
// using Factory.ViewModels;

namespace Factory.Controllers
{
    public class AccountController : Controller
    {
        private readonly FactoryContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, FactoryContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        // [HttpPost]
    }
}