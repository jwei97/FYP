using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FYP.Models;
using static FYP.Helper.ConstantManager;
using Microsoft.AspNetCore.Http;
using FYP.Helper;

namespace FYP.Controllers
{
    public class LecturerProfileController : Controller
    {
        private readonly FYPContext _context;

        public LecturerProfileController(FYPContext context)
        {
            _context = context;
        }
        

        // GET: AdminAccounts/Details/5
        public async Task<IActionResult> Index()
        {
            var account = await _context.Account
                .FirstOrDefaultAsync(m => m.ID == int.Parse(HttpContext.Session.GetString(HttpSessionName.CURRENT_ID)));
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: AdminAccounts/Edit/5
        public async Task<IActionResult> Edit()
        {
            var account = await _context.Account.FindAsync(int.Parse(HttpContext.Session.GetString(HttpSessionName.CURRENT_ID)));
            if (account == null)
            {
                return NotFound();
            }
            ViewData["Type"] = new SelectList(SecurityConstants.RoleType);
            return View(account);
        }

        // POST: AdminAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("ID,Username,Password,Name,Type,Deleted")] Account account)
        {
            if (int.Parse(HttpContext.Session.GetString(ConstantManager.HttpSessionName.CURRENT_ID)) != account.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.ID))
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
            ViewData["Type"] = new SelectList(SecurityConstants.RoleType);
            return View(account);
        }
        
        private bool AccountExists(int id)
        {
            return _context.Account.Any(e => e.ID == id);
        }
    }
}
