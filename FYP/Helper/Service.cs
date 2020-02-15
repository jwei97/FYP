using FYP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Helper
{
    public class Service
    {


        public static async Task LogAccessDocument(FYPContext _context, Account account, Document document)
        {
            TracingHistory t = new TracingHistory();
            t.AccessTime = DateTime.Now;
            t.DocId = document.ID;
            t.DocName = document.DocName;
            t.Description = document.Description;
            t.FileURL = document.FileURL;
            t.UserId = account.ID;
            t.Username = account.Username;
            t.Name = account.Name;
            t.Type = account.Type;
            _context.Add(t);
            await _context.SaveChangesAsync();
        }



        public static async Task LogLogin(FYPContext _context, Account account)
        {
            LogHistory t = new LogHistory();
            t.AccessTime = DateTime.Now;
            t.Userid = account.ID;
            t.Username = account.Username;
            t.Name = account.Name;
            t.Type = account.Type;
            _context.Add(t);
            await _context.SaveChangesAsync();
        }
    }
}
