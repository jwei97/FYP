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
    public class AdminTracingHistoriesController : Controller
    {
        private readonly FYPContext _context;

        public AdminTracingHistoriesController(FYPContext context)
        {
            _context = context;
        }

        // GET: AdminTracingHistories
        public async Task<IActionResult> Index()
        {
            return View(await _context.TracingHistory.ToListAsync());
        }

        // GET: AdminTracingHistories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tracingHistory = await _context.TracingHistory
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tracingHistory == null)
            {
                return NotFound();
            }

            return View(tracingHistory);
        }

        // GET: AdminTracingHistories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminTracingHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,AccessTime,DocId,DocName,Description,FileURL,UserId,Username,Name,Type,Deleted")] TracingHistory tracingHistory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tracingHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tracingHistory);
        }

        // GET: AdminTracingHistories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tracingHistory = await _context.TracingHistory.FindAsync(id);
            if (tracingHistory == null)
            {
                return NotFound();
            }
            return View(tracingHistory);
        }

        // POST: AdminTracingHistories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,AccessTime,DocId,DocName,Description,FileURL,UserId,Username,Name,Type,Deleted")] TracingHistory tracingHistory)
        {
            if (id != tracingHistory.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tracingHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TracingHistoryExists(tracingHistory.ID))
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
            return View(tracingHistory);
        }

        // GET: AdminTracingHistories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tracingHistory = await _context.TracingHistory
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tracingHistory == null)
            {
                return NotFound();
            }

            return View(tracingHistory);
        }

        // POST: AdminTracingHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tracingHistory = await _context.TracingHistory.FindAsync(id);
            _context.TracingHistory.Remove(tracingHistory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TracingHistoryExists(int id)
        {
            return _context.TracingHistory.Any(e => e.ID == id);
        }
    }
}
