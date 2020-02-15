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
    public class LecturerAccountModulesController : Controller
    {
        private readonly FYPContext _context;

        public LecturerAccountModulesController(FYPContext context)
        {
            _context = context;
        }

        // GET: AdminAccountModules
        public async Task<IActionResult> Index()
        {
            var fYPContext = _context.AccountModule.Include(a => a.Account).Include(a => a.Module);
            return View(await fYPContext.ToListAsync());
        }

        // GET: AdminAccountModules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountModule = await _context.AccountModule
                .Include(a => a.Account)
                .Include(a => a.Module)
                .FirstOrDefaultAsync(m => m.ModuleId == id);
            if (accountModule == null)
            {
                return NotFound();
            }

            return View(accountModule);
        }

        // GET: AdminAccountModules/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Account.Where(x => x.Type == SecurityConstants.ACCOUNT_TYPE_STUDENT), "ID", "Username");
            ViewData["ModuleId"] = new SelectList(_context.Module, "ID", "ModuleName");
            return View();
        }

        // POST: AdminAccountModules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,ModuleId,Deleted")] AccountModule accountModule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(accountModule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Account.Where(x => x.Type == SecurityConstants.ACCOUNT_TYPE_STUDENT), "ID", "Password", accountModule.AccountId);
            ViewData["ModuleId"] = new SelectList(_context.Module, "ID", "ModuleName", accountModule.ModuleId);
            return View(accountModule);
        }

        // GET: AdminAccountModules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountModule = await _context.AccountModule.FindAsync(id);
            if (accountModule == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Account.Where(x => x.Type == SecurityConstants.ACCOUNT_TYPE_STUDENT), "ID", "Password", accountModule.AccountId);
            ViewData["ModuleId"] = new SelectList(_context.Module, "ID", "ModuleName", accountModule.ModuleId);
            return View(accountModule);
        }

        // POST: AdminAccountModules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,ModuleId,Deleted")] AccountModule accountModule)
        {
            if (id != accountModule.ModuleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(accountModule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountModuleExists(accountModule.ModuleId))
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
            ViewData["AccountId"] = new SelectList(_context.Account.Where(x => x.Type == SecurityConstants.ACCOUNT_TYPE_STUDENT), "ID", "Password", accountModule.AccountId);
            ViewData["ModuleId"] = new SelectList(_context.Module, "ID", "ModuleName", accountModule.ModuleId);
            return View(accountModule);
        }

        // GET: AdminAccountModules/Delete/5
        public async Task<IActionResult> Delete(int? ModuleId, int? AccountId)
        {
            if (ModuleId == null || AccountId == null)
            {
                return NotFound();
            }
            var find = await _context.AccountModule.Where(x => x.ModuleId == ModuleId).Where(x => x.AccountId == AccountId).FirstOrDefaultAsync();
            var accountModule = _context.AccountModule.Remove(find);
            await _context.SaveChangesAsync();
            if (accountModule == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: AdminAccountModules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var accountModule = await _context.AccountModule.FindAsync(id);
            _context.AccountModule.Remove(accountModule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountModuleExists(int id)
        {
            return _context.AccountModule.Any(e => e.ModuleId == id);
        }
    }
}
