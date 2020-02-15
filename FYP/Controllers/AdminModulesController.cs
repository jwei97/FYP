using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FYP.Models;

namespace FYP.Controllers
{
    public class AdminModulesController : Controller
    {
        private readonly FYPContext _context;

        public AdminModulesController(FYPContext context)
        {
            _context = context;
        }

        // GET: AdminModules
        public async Task<IActionResult> Index()
        {
            var fYPContext = _context.Module.Include(x => x.Course);
            return View(await fYPContext.ToListAsync());
        }

        // GET: AdminModules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var xmodule = await _context.Module
                .Include(x => x.Course)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (xmodule == null)
            {
                return NotFound();
            }

            return View(xmodule);
        }

        // GET: AdminModules/Create
        public IActionResult Create()
        {
            ViewData["CourseID"] = new SelectList(_context.Course, "ID", "CourseName");
            return View();
        }

        // POST: AdminModules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ModuleName,Description,CourseID,Deleted")] Module xmodule)
        {
            ViewData["CourseID"] = new SelectList(_context.Course, "ID", "CourseName", xmodule.CourseID);
            if (ModelState.IsValid)
            {

                var find = _context.Module.Where(x => x.ModuleName == xmodule.ModuleName).FirstOrDefault();
                if (find != null)
                {
                    ModelState.AddModelError("", "This '" + xmodule.ModuleName + "' have been added, pls select other one ");
                    return View(xmodule);
                }
                _context.Add(xmodule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(xmodule);
        }

        // GET: AdminModules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var xmodule = await _context.Module.FindAsync(id);
            if (xmodule == null)
            {
                return NotFound();
            }
            ViewData["CourseID"] = new SelectList(_context.Course, "ID", "CourseName", xmodule.CourseID);
            return View(xmodule);
        }

        // POST: AdminModules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ModuleName,Description,CourseID,Deleted")] Module xmodule)
        {
            if (id != xmodule.ID)
            {
                return NotFound();
            }

            ViewData["CourseID"] = new SelectList(_context.Course, "ID", "CourseName", xmodule.CourseID);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(xmodule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuleExists(xmodule.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(xmodule);
        }

        // GET: AdminModules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var xmodule = await _context.Module
                .Include(x => x.Course)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (xmodule == null)
            {
                return NotFound();
            }

            return View(xmodule);
        }

        // POST: AdminModules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var xmodule = await _context.Module.FindAsync(id);
            _context.Module.Remove(xmodule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModuleExists(int id)
        {
            return _context.Module.Any(e => e.ID == id);
        }
    }
}
