using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FYP.Models;
using static FYP.Helper.ConstantManager;

namespace FYP.Controllers
{
    public class AdminLogHistoriesController : Controller
    {
        private readonly FYPContext _context;

        public AdminLogHistoriesController(FYPContext context)
        {
            _context = context;
        }

        // GET: AdminLogHistories
        public async Task<IActionResult> Index()
        {
            return View(await _context.LogHistory.Where(x => x.Type != SecurityConstants.ACCOUNT_TYPE_ADMIN).ToListAsync());
        }

        // GET: AdminLogHistories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logHistory = await _context.LogHistory
                .FirstOrDefaultAsync(m => m.ID == id);
            if (logHistory == null)
            {
                return NotFound();
            }

            return View(logHistory);
        }

        // GET: AdminLogHistories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminLogHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Userid,Username,Name,Type,AccessTime,Deleted")] LogHistory logHistory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(logHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(logHistory);
        }

        // GET: AdminLogHistories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logHistory = await _context.LogHistory.FindAsync(id);
            if (logHistory == null)
            {
                return NotFound();
            }
            return View(logHistory);
        }

        // POST: AdminLogHistories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Userid,Username,Name,Type,AccessTime,Deleted")] LogHistory logHistory)
        {
            if (id != logHistory.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(logHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LogHistoryExists(logHistory.ID))
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
            return View(logHistory);
        }

        // GET: AdminLogHistories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logHistory = await _context.LogHistory
                .FirstOrDefaultAsync(m => m.ID == id);
            if (logHistory == null)
            {
                return NotFound();
            }

            return View(logHistory);
        }

        // POST: AdminLogHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var logHistory = await _context.LogHistory.FindAsync(id);
            _context.LogHistory.Remove(logHistory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LogHistoryExists(int id)
        {
            return _context.LogHistory.Any(e => e.ID == id);
        }
    }
}
