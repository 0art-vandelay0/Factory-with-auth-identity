using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Factory.Models;
using System.Linq;

namespace Factory.Controllers
{
    public class EngineersController : Controller
    {
        private readonly FactoryContext _db;

        public EngineersController(FactoryContext db)
        {
            _db = db;
        }

        public ActionResult Index()
        {
            return View(_db.Engineers.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Engineer engineer)
        {
            if (!ModelState.IsValid)
            {
                return View(engineer);
            }
            else
            {
                _db.Engineers.Add(engineer);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Details(int id)
        {
            Engineer thisEngineer = _db.Engineers
                .Include(engineer => engineer.JoinEntities)
                .ThenInclude(join => join.Machine)
                .FirstOrDefault(engineer => engineer.EngineerId == id);
            return View(thisEngineer);
        }

        public ActionResult AddMachine(int id)
        {
            Engineer thisEngineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == id);
            ViewBag.MachineId = new SelectList(_db.Machines, "MachineId", "Name");
            return View(thisEngineer);
        }

        [HttpPost]
        public ActionResult AddMachine(Engineer engineer, int MachineId)
        {
#nullable enable
            EngineerMachine? joinEntity = _db.EngineerMachine.FirstOrDefault(join => join.EngineerId == engineer.EngineerId && join.MachineId == MachineId);
#nullable disable
            if (joinEntity == null && MachineId != 0)
            {
                _db.EngineerMachine.Add(new EngineerMachine() { EngineerId = engineer.EngineerId, MachineId = MachineId });
                _db.SaveChanges();
            }
            return RedirectToAction("Details", new { id = engineer.EngineerId });
        }

        public ActionResult Edit(int id)
        {
            Engineer thisEngineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == id);
            ViewBag.MachineId = new SelectList(_db.Machines, "MachineId", "Name");
            return View(thisEngineer);
        }

        [HttpPost]
        public ActionResult Edit(Engineer engineer)
        {
            if (!ModelState.IsValid)
            {
                return View(engineer);
            }
            else
            {
                _db.Entry(engineer).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index", new { id = engineer.EngineerId });
            }
        }

        public ActionResult Delete(int id)
        {
            Engineer thisEngineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == id);
            return View(thisEngineer);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Engineer thisEngineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == id);
            _db.Engineers.Remove(thisEngineer);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

    }

}