using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BidZ.lk.Models;
using Microsoft.AspNet.Identity;

namespace BidZ.lk.Controllers
{
    public class UserMessagesController : Controller
    {
        private AuctionsDataContext db = new AuctionsDataContext();
        private ApplicationDbContext DB = new ApplicationDbContext();

        public ActionResult MessageRecieveGet(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var result = DB.Users.Find(id);
                Session["IdOfMessage"] = result.Email;
                return RedirectToAction("Create");
            }

        }

        [HttpPost]
        public ActionResult ReplyForSeller(string id)
        {
            Session["IdOfMessage"] = id;
            return RedirectToAction("Create");
        }

        // GET: UserMessages
        [Authorize(Users = "Admin@Bidz.lk")]
        public async Task<ActionResult> Index()
        {
            return View(await db.Message.ToListAsync());
        }

        // GET: UserMessages/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Messages messages = await db.Message.FindAsync(id);
            if (messages == null)
            {
                return HttpNotFound();
            }
            return View(messages);
        }

        // GET: UserMessages/Create
        public ActionResult Create()
        {
            return View();
        }

        //customer can view whatever he has
        public ActionResult SearchSendersUserEmail()
        {
            var userID = User.Identity.GetUserName();
            AuctionsDataContext db = new AuctionsDataContext();
            var result = db.Message.Where(c => c.SenderUserEmail.StartsWith(userID));
            return View(result.ToList());
        }

        //seller can view whatever he has
        public ActionResult SenderRecieversEmail()
        {
            var userID = User.Identity.GetUserName();
            AuctionsDataContext db = new AuctionsDataContext();
            var result = db.Message.Where(c => c.RecieverUsername.StartsWith(userID));
            return View(result.ToList());
        }

        //Both users can view whatever they have
        public ActionResult BothUsersCanView()
        {
            var userID = User.Identity.GetUserName();
            AuctionsDataContext db = new AuctionsDataContext();
            var result = db.Message.Where(c => c.RecieverUsername.StartsWith(userID) || c.SenderUserEmail.StartsWith(userID));
            return View(result.ToList());
        }

        public ActionResult SendingManual()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Users = "Admin@Bidz.lk")]
        public ActionResult AllMessages()
        {
            AuctionsDataContext db = new AuctionsDataContext();
            var result = db.Message.ToList();
            return View(result);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendingManual([Bind(Include = "Id,SenderUserEmail,reciverID,Contents")] Messages messages)
        {
            if (ModelState.IsValid)
            {
                db.Message.Add(messages);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(messages);
        }

        public ActionResult Results()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,SenderUserEmail,RecieverUsername,Contents")] Messages messages)
        {
            if (ModelState.IsValid)
            {
                db.Message.Add(messages);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            return View(messages);
        }

        // GET: UserMessages/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Messages messages = await db.Message.FindAsync(id);
            if (messages == null)
            {
                return HttpNotFound();
            }
            return View(messages);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,SenderUserEmail,RecieverUsername,Contents")] Messages messages)
        {
            if (ModelState.IsValid)
            {
                db.Entry(messages).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(messages);
        }

        // GET: UserMessages/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Messages messages = await db.Message.FindAsync(id);
            if (messages == null)
            {
                return HttpNotFound();
            }
            return View(messages);
        }

        // POST: UserMessages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Messages messages = await db.Message.FindAsync(id);
            db.Message.Remove(messages);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
