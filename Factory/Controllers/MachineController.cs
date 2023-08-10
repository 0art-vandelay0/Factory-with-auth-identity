using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Factory.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Factory.Controllers
{
    public class MachinesController : Controller
    {
        private readonly FactoryContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public MachinesController(UserManager<ApplicationUser> userManager, FactoryContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        // public ActionResult Index()
        // {
        //     return View(_db.Machines.ToList());
        // }

        public async Task<ActionResult> Index()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
            List<Machine> userMachines = _db.Machines.Where(entry => entry.User.Id == currentUser.Id).Include(machine => machine.JoinEntities).ThenInclude(join => join.Engineer).ToList();
            return View(userMachines);
        }

        public ActionResult Create()
        {
            ViewBag.EngineerId = new SelectList(_db.Engineers, "EngineerId", "Name");
            return View();
        }

        // [HttpPost]
        // public ActionResult Create(Machine machine)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return View(machine);
        //     }
        //     else
        //     {
        //         _db.Machines.Add(machine);
        //         _db.SaveChanges();
        //         return RedirectToAction("Index");
        //     }
        // }

        [HttpPost]
        public async Task<ActionResult> Create(Machine machine, int EngineerId)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.EngineerId = new SelectList(_db.Engineers, "EngineerId", "Name");
                return View(machine);
            }
            else
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
                machine.User = currentUser;
                _db.Machines.Add(machine);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Details(int id)
        {
            Machine thisMachine = _db.Machines
                .Include(machine => machine.JoinEntities)
                .ThenInclude(join => join.Engineer)
                .FirstOrDefault(machine => machine.MachineId == id);
            return View(thisMachine);
        }

        public ActionResult AddEngineer(int id)
        {
            Machine thisMachine = _db.Machines.FirstOrDefault(machine => machine.MachineId == id);
            ViewBag.EngineerId = new SelectList(_db.Engineers, "EngineerId", "Name");
            return View(thisMachine);
        }

        [HttpPost]
        public ActionResult AddEngineer(Machine machine, int EngineerId)
        {
#nullable enable
            EngineerMachine? joinEntity = _db.EngineerMachine.FirstOrDefault(join => join.MachineId == machine.MachineId && join.EngineerId == EngineerId);
#nullable disable
            if (joinEntity == null && EngineerId != 0)
            {
                _db.EngineerMachine.Add(new EngineerMachine() { EngineerId = EngineerId, MachineId = machine.MachineId });
                _db.SaveChanges();
            }
            return RedirectToAction("Details", new { id = machine.MachineId });
        }

        public ActionResult Edit(int id)
        {
            Machine thisMachine = _db.Machines.FirstOrDefault(machine => machine.MachineId == id);
            ViewBag.EngineerId = new SelectList(_db.Engineers, "EngineerId", "Name");
            return View(thisMachine);
        }

        [HttpPost]
        public ActionResult Edit(Machine machine)
        {
            if (!ModelState.IsValid)
            {
                return View(machine);
            }
            else
            {
                _db.Entry(machine).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Details", new { id = machine.MachineId });
            }
        }

        public ActionResult Delete(int id)
        {
            Machine thisMachine = _db.Machines.FirstOrDefault(machine => machine.MachineId == id);
            return View(thisMachine);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Machine thisMachine = _db.Machines.FirstOrDefault(machine => machine.MachineId == id);
            _db.Machines.Remove(thisMachine);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

    }

}