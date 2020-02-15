using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FYP.Helper;
using FYP.Models;
using static FYP.Helper.ConstantManager;

namespace RMS.Controllers
{
    public class LoginController : Controller
    {

        private readonly FYPContext _context;

        public LoginController(FYPContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            Account c = new Account();
            c.Username = HttpContext.Session.GetString(ConstantManager.HttpSessionName.CURRENT_USERNAME);
            c.Password = HttpContext.Session.GetString(ConstantManager.HttpSessionName.CURRENT_PASSWORD);

            var result = await _context.Account
                .Where(m => m.Username == c.Username)
                .Where(m => m.Password == c.Password)
                .Where(m => m.Deleted == false)
                .FirstOrDefaultAsync();


            if (result == null)
            {
                return View();
            }
            
            await Service.LogLogin(_context, result);

            switch (result.Type)
            {
                case SecurityConstants.ACCOUNT_TYPE_ADMIN:
                    return RedirectToAction("Index", "AdminAccounts");
                case SecurityConstants.ACCOUNT_TYPE_LECTURE:
                    return RedirectToAction("Index", "LecturerDocuments"); 
                case SecurityConstants.ACCOUNT_TYPE_STUDENT:
                    return RedirectToAction("Index", "StudentDocuments");
                default:
                    return View();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Account c)
        {

            var result = await _context.Account
                .Where(m => m.Username == c.Username)
                .Where(m => m.Password == c.Password)
                .Where(m => m.Deleted == false)
                .FirstOrDefaultAsync();
            if (result == null)
            {
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
                return View();
            }

            await Service.LogLogin(_context, result);

            HttpContext.Session.SetString(ConstantManager.HttpSessionName.CURRENT_USERNAME, result.Username);
            HttpContext.Session.SetString(ConstantManager.HttpSessionName.CURRENT_PASSWORD, result.Password);
            HttpContext.Session.SetString(ConstantManager.HttpSessionName.CURRENT_TYPE, result.Type);
            HttpContext.Session.SetString(ConstantManager.HttpSessionName.CURRENT_ID, result.ID.ToString());

            switch (result.Type)
            {
                case SecurityConstants.ACCOUNT_TYPE_ADMIN:
                    return RedirectToAction("Index", "AdminAccounts");
                case SecurityConstants.ACCOUNT_TYPE_LECTURE:
                    return RedirectToAction("Pending", "LecturerDocuments");
                case SecurityConstants.ACCOUNT_TYPE_STUDENT:
                    return RedirectToAction("Index", "StudentDocuments");
                default:
                    return View();
            }
        }

        
        public ActionResult Logout()
        {
            HttpContext.Session.Remove(ConstantManager.HttpSessionName.CURRENT_USERNAME);
            HttpContext.Session.Remove(ConstantManager.HttpSessionName.CURRENT_PASSWORD);
            HttpContext.Session.Remove(ConstantManager.HttpSessionName.CURRENT_TYPE);
            HttpContext.Session.Remove(ConstantManager.HttpSessionName.CURRENT_ID);

            return RedirectToAction("Index","Login");
        }
    }
}