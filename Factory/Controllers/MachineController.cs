using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Factory.Models;
using System.Linq;

namespace Factory.Controllers
{
    public class MachinesController : Controller
    {
        private readonly FactoryContext _db;

        public MachinesController(FactoryContext db)
        {
            _db = db;
        }

        public ActionResult Index()
        {
            return View(_db.Machines.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.EngineerId = new SelectList(_db.Engineers, "EngineerId", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Machine machine)
        {
            if (!ModelState.IsValid)
            {
                return View(machine);
            }
            else
            {
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