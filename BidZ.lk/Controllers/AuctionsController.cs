using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BidZ.lk.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.IO;

namespace BidZ.lk.Controllers
{
    public class AuctionsController : Controller
    {
        private AuctionsDataContext db = new AuctionsDataContext();
        private ApplicationDbContext DB = new ApplicationDbContext();
        // GET: Auctions
        public ActionResult Index()
        {
            var db = new AuctionsDataContext();
            var auctions = db.Auctions.ToArray();
            return View(auctions);
        }

        [Authorize(Users = "Admin@Bidz.lk")]
        public ActionResult IndexForAdmin()
        {
            var db = new AuctionsDataContext();
            var auctions = db.Auctions.ToArray();
            return View(auctions);
        }


        [Authorize]
        public ActionResult Auction(long id)
        {
            var db = new AuctionsDataContext();
            TempData["id"] = id;
            var auction = db.Auctions.Find(id);
            return View(auction);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Bid(Bid bid)
        {
            var db = new AuctionsDataContext();
            var auction = db.Auctions.Find(bid.AuctionId);

            if (auction == null)
            {
                ModelState.AddModelError("AuctionId", "Auction not found!");
            }
            else if (auction.CurrentPrice >= bid.Amount)
            {
                ModelState.AddModelError("amount", "Bid amount must exceed current bid !");
                return RedirectToAction("Auction", new { id = bid.AuctionId });
            }
            else
            {
                if (auction.StartPrice < bid.Amount)
                {
                    // auction calculation end time...
                    System.TimeSpan ts = new TimeSpan(auction.EndTime.Ticks - DateTime.Now.Ticks);
                    if (0 <= ts.Days)
                    {
                        bid.Username = User.Identity.Name;
                        auction.Bids.Add(bid);
                        auction.CurrentPrice = bid.Amount;
                        db.SaveChanges();
                    }
                    else if (0 > ts.Days)
                    {
                        try {
                            TempData["status"] = "Sorry the bid has been expired!";
                            var su = auction.Bids.Last(c => c.AuctionId == bid.AuctionId);
                            TempData["Winner"] = su.Username;
                            return RedirectToAction("Auction", new { id = bid.AuctionId });
                        }catch
                        {
                            TempData["status"] = "Sorry the bid has been expired!";
                            return RedirectToAction("Auction", new { id = bid.AuctionId });
                        }
                    }
                    else
                    {

                        TempData["status"] = "Sorry the bid has been expired!";
                        var su = auction.Bids.Last(c => c.AuctionId == bid.AuctionId);
                        TempData["Winner"] = su.Username;
                        return RedirectToAction("Auction", new { id = bid.AuctionId });

                    }
                }
                else
                {
                    ViewData["Data"] = "Your bid amout must exeed start price!";
                    //ModelState.AddModelError("amount", "Your bid amout must exeed start price!");
                    return RedirectToAction("Auction", new { id = bid.AuctionId });

                }
            }
            if (!Request.IsAjaxRequest())
            {
                return RedirectToAction("Auction", new { id = bid.AuctionId });
            }
            else
            {
                var httpStatus = ModelState.IsValid ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
                return new HttpStatusCodeResult(httpStatus);
            }
        }



        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            var protypeList = new SelectList(new[] { "jewellery", "Automobile", "RealEstate" });
            ViewBag.ProtypeList = protypeList;
            return View();
        }



        [HttpPost]
        [Authorize]
        public ActionResult Create([Bind(Exclude = "CurrentPrice")]Models.Auction auction)
        {
            if (ModelState.IsValid)
            {
                if (auction.StartPrice < auction.productPrice)
                {
                    var db = new AuctionsDataContext();
                    db.Auctions.Add(auction);
                    db.SaveChanges();
                    Session["productID"] = auction.Id;
                    return RedirectToAction("ProductPicture");
                }
                else
                {
                    ViewBag.Error = "Your start price should be lower than your reserve price";
                    return Create();
                }

            }
            return Create();
        }



        [Authorize]
        public ActionResult SuccessFullyCreated()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult ProductPicture()
        {
            return View();
        }


        [HttpPost]
        [Authorize]
        public ActionResult ProductPicture(HttpPostedFileBase postedFile)
        {
            if (postedFile != null)
            {

                string path = Server.MapPath("~/Content/Uploader/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                else
                {
                    var ses = Session["productID"];
                    if (ses == null)
                    {
                        ViewBag.Message = "You have not registered any product item";
                    }
                    else
                    {
                        string saveName = Path.GetFileName(postedFile.FileName);
                        postedFile.SaveAs(path + saveName);
                        int u = Convert.ToInt32(ses);
                        var found = db.Auctions.Find(u);
                        var x = saveName;
                        found.ImageURL = "~/Content/Uploader/" + x;
                        db.SaveChanges();
                        ViewBag.Message = "Your Picture has updated to your product.Please go to home page!";
                    }
                }
            }
            return RedirectToAction("SuccessFullyCreated");
        }



        public ActionResult BidWinner(Bid bid)
        {
            Bid act = new Bid();
            using (AuctionsDataContext db = new AuctionsDataContext())
            {
                act = db.Bids.Where(c => c.AuctionId == bid.AuctionId).SingleOrDefault();
            }
            return View(act);
        }

        [Authorize]
        public ActionResult Edit(int? id)
        {
            Auction act = new Auction();
            using (AuctionsDataContext db = new AuctionsDataContext())
            {
                act = db.Auctions.Where(m => m.Id == id).FirstOrDefault();
            }
            return View(act);

        }


        [HttpPost]
        [Authorize]
        public ActionResult Edit(Auction act, FormCollection fc)
        {
            using (AuctionsDataContext db = new AuctionsDataContext())
            {
                db.Auctions.Add(act);
                db.Entry<Auction>(act).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return View(act);
        }

        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Auction auction = db.Auctions.Find(id);
            if (auction == null)
            {
                return HttpNotFound();
            }
            return View(auction);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Auction auction = db.Auctions.Find(id);
            db.Auctions.Remove(auction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize]
        public ActionResult ViewMyBids()
        {
            var db = new AuctionsDataContext();
            var pKey = User.Identity.GetUserId();
            var result = db.Auctions
                            .Where(p => p.SellerUserID == pKey)
                            .ToList();
            return View(result.ToList());
        }

        [Authorize]
        public ActionResult AuctionSellerDetails(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = DB.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }
    }
}