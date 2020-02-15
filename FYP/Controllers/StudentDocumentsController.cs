using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FYP.Models;
using Microsoft.WindowsAzure.Storage.Blob;
using RMS.Helper;
using Microsoft.AspNetCore.Http;
using static FYP.Helper.ConstantManager;
using FYP.Helper;

namespace FYP.Controllers
{
    public class StudentDocumentsController : Controller
    {
        private readonly FYPContext _context;

        public StudentDocumentsController(FYPContext context)
        {
            _context = context;
        }

        // GET: AdminDocuments
        public async Task<IActionResult> Index()
        {
            var userid = HttpContext.Session.GetString(ConstantManager.HttpSessionName.CURRENT_ID);
            var listmodule = await _context.AccountModule.Where(x => x.AccountId == int.Parse(userid)).Select(x => x.ModuleId).ToListAsync();

            var fYPContext = await _context.Document
                .Include(d => d.AccountUpload)
                .Include(d => d.Module)
                .Where(d => d.Status == DocumentStatusConstants.SUCCESS)
                .Where(d => listmodule.Contains(d.ModuleId)).ToListAsync();
            return View(fYPContext);
        }

        // GET: AdminDocuments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Document
                .Include(d => d.AccountUpload)
                .Include(d => d.Module)
                .FirstOrDefaultAsync(m => m.ID == id);

            var account = await _context.Account.FindAsync(int.Parse(HttpContext.Session.GetString(HttpSessionName.CURRENT_ID)));
            await Service.LogAccessDocument(_context, account, document);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }
        
        // GET: AdminDocuments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Document.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }
            
            ViewData["Status"] = new SelectList(DocumentStatusConstants.statusList);
            ViewData["ModuleId"] = new SelectList(_context.Module, "ID", "ModuleName", document.ModuleId);
            return View(document);
        }

        // POST: AdminDocuments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  Document document, IFormFile files)
        {
            if (id != document.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (files != null)
                    {
                        CloudBlobContainer container = await CloudHelper.GetCloudBlobContainerAsync("product-container");
                        CloudBlockBlob blob = container.GetBlockBlobReference(Guid.NewGuid().ToString());
                        blob.UploadFromStreamAsync(files.OpenReadStream()).Wait();
                        document.FileURL = blob.Uri.ToString();
                    }
                    _context.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(document.ID))
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
            
            ViewData["Status"] = new SelectList(DocumentStatusConstants.statusList);
            
            ViewData["ModuleId"] = new SelectList(_context.Module, "ID", "ModuleName", document.ModuleId);
            return View(document);
        }

        // GET: AdminDocuments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Document
                
                .Include(d => d.AccountUpload)
                .Include(d => d.Module)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: AdminDocuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var document = await _context.Document.FindAsync(id);
            _context.Document.Remove(document);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentExists(int id)
        {
            return _context.Document.Any(e => e.ID == id);
        }
    }
}
