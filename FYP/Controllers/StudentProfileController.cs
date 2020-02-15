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
    public class StudentProfileController : Controller
    {
        private readonly FYPContext _context;

        public StudentProfileController(FYPContext context)
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
        
        private bool AccountExists(int id)
        {
            return _context.Account.Any(e => e.ID == id);
        }
    }
}
