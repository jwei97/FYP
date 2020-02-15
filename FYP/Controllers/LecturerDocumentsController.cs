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
    public class LecturerDocumentsController : Controller
    {
        private readonly FYPContext _context;

        public LecturerDocumentsController(FYPContext context)
        {
            _context = context;
        }

        // GET: AdminDocuments
        public async Task<IActionResult> Index()
        {
            var fYPContext = _context.Document
                .Where(x => x.Status == DocumentStatusConstants.SUCCESS)
                .Include(d => d.AccountUpload)
                .Include(d => d.Module);
            return View(await fYPContext.ToListAsync());
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
        public async Task<IActionResult> Create(Document document, IFormFile files)
        {
            if (ModelState.IsValid)
            {
                CloudBlobContainer container = await CloudHelper.GetCloudBlobContainerAsync("product-container");
                CloudBlockBlob blob = container.GetBlockBlobReference(Guid.NewGuid().ToString());
                blob.UploadFromStreamAsync(files.OpenReadStream()).Wait();
                document.FileURL = blob.Uri.ToString();

                document.UploadById = int.Parse(HttpContext.Session.GetString(HttpSessionName.CURRENT_ID));
                document.Status = DocumentStatusConstants.SUCCESS;



                _context.Add(document);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Status"] = new SelectList(DocumentStatusConstants.statusList);

            ViewData["UploadById"] = new SelectList(_context.Account, "ID", "Username");
            ViewData["ModuleId"] = new SelectList(_context.Module, "ID", "ModuleName", document.ModuleId);
            return View(document);
        }


        // GET: AdminDocuments
        public async Task<IActionResult> Pending()
        {
            var fYPContext = _context.Document
                .Where(x => x.Status == DocumentStatusConstants.PENDING)
                .Include(d => d.AccountUpload)
                .Include(d => d.Module);
            return View(await fYPContext.ToListAsync());
        }

        // GET: AdminDocuments/Details/5
        public async Task<IActionResult> SetSuccess(int? id)
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

            try
            {
                document.Status = DocumentStatusConstants.SUCCESS;
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
            return RedirectToAction(nameof(Pending));
        }


        // GET: AdminDocuments/Details/5
        public async Task<IActionResult> SetCancel(int? id)
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

            try
            {
                document.Status = DocumentStatusConstants.CANCEL;
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
            return RedirectToAction(nameof(Pending));
        }

        private bool DocumentExists(int id)
        {
            return _context.Document.Any(e => e.ID == id);
        }
    }
}
