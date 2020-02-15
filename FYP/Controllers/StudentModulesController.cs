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
    public class StudentModulesController : Controller
    {
        private readonly FYPContext _context;

        public StudentModulesController(FYPContext context)
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
        
        private bool ModuleExists(int id)
        {
            return _context.Module.Any(e => e.ID == id);
        }
    }
}
