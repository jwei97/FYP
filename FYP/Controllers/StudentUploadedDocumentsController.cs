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

namespace FYP.Controllers
{
    public class StudentUploadedDocumentsController : Controller
    {
        private readonly FYPContext _context;

        public StudentUploadedDocumentsController(FYPContext context)
        {
            _context = context;
        }

        // GET: AdminDocuments
        public async Task<IActionResult> Index()
        {
            var fYPContext = _context.Document
                .Where(x => x.UploadById == int.Parse(HttpContext.Session.GetString(HttpSessionName.CURRENT_ID)))
                .Include(d => d.AccountUpload)
                .Include(d => d.Module);

            var a =  await fYPContext.ToListAsync();
            return View(a);
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
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // GET: AdminDocuments/Create
        public IActionResult Create()
        {
            ViewData["Status"] = new SelectList(DocumentStatusConstants.statusList);
            ViewData["UploadById"] = new SelectList(_context.Account, "ID", "Username");
            ViewData["ModuleId"] = new SelectList(_context.Module, "ID", "ModuleName");
            return View();
        }

        // POST: AdminDocuments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Document document, IFormFile files)
        {
            if (ModelState.IsValid)
            {
                CloudBlobContainer container = await CloudHelper.GetCloudBlobContainerAsync("product-container");
                CloudBlockBlob blob = container.GetBlockBlobReference(Guid.NewGuid().ToString());
                blob.UploadFromStreamAsync(files.OpenReadStream()).Wait();
                document.FileURL = blob.Uri.ToString();

                document.UploadById = int.Parse(HttpContext.Session.GetString(HttpSessionName.CURRENT_ID));
                document.Status = DocumentStatusConstants.PENDING;



                _context.Add(document);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["Status"] = new SelectList(DocumentStatusConstants.statusList);

            ViewData["UploadById"] = new SelectList(_context.Account, "ID", "Username");
            ViewData["ModuleId"] = new SelectList(_context.Module, "ID", "ModuleName", document.ModuleId);
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

            ViewData["UploadById"] = new SelectList(_context.Account, "ID", "Username");
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


                    document.UploadById = int.Parse(HttpContext.Session.GetString(HttpSessionName.CURRENT_ID));
                    document.Status = DocumentStatusConstants.PENDING;

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
            ViewData["UploadById"] = new SelectList(_context.Account, "ID", "Username", document.UploadById);
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
