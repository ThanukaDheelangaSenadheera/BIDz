using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BidZ.lk.Models;
using System.IO;

namespace BidZ.lk.Controllers
{
    public class LiveAuctionsController : Controller
    {
        private AuctionsDataContext db = new AuctionsDataContext();

        [Authorize]
        public ActionResult Index()
        {
            return View(db.LiveAuctions.ToList());
        }

        [Authorize]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LiveAuction liveAuction = db.LiveAuctions.Find(id);
            if (liveAuction == null)
            {
                return HttpNotFound();
            }
            return View(liveAuction);
        }

        [Authorize]
        public ActionResult Create()
        {
            var protypeList = new SelectList(new[] { "jewellery", "Automobile", "RealEstate" });
            ViewBag.ProtypeList = protypeList;
            return View();
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,productName,productPrice,productType,productAddress,StartTime,EndTime,CurrentPrice,ImageURL,SellerUserID,status,countLiveBid")] LiveAuction liveAuction)
        {
            if (ModelState.IsValid)
            {
                var db = new AuctionsDataContext();
                db.LiveAuctions.Add(liveAuction);
                db.SaveChanges();
                Session["productID"] = liveAuction.Id;
                return RedirectToAction("ProductPicture");
            }
            return Create();
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
                        var found = db.LiveAuctions.Find(u);
                        var x = saveName;
                        found.ImageURL = "~/Content/Uploader/" + x;
                        db.SaveChanges();
                        ViewBag.Message = "Your Picture has updated to your product.Please go to home page!";
                    }
                }
            }
            return RedirectToAction("SuccessFullyCreated");
        }



        public ActionResult SuccessFullyCreated()
        {
            return View();
        }


        [Authorize]
        public ActionResult LiveAuction(long id)
        {
            var db = new AuctionsDataContext();
            TempData["id"] = id;
            var auction = db.LiveAuctions.Find(id);
            return View(auction);
        }

        [Authorize]
        [HttpPost]
        public ActionResult LiveBid(LiveBid lbid)
        {
            var db = new AuctionsDataContext();
            var auction = db.LiveAuctions.Find(lbid.LiveAuctionId);

            if (auction == null)
            {
                ModelState.AddModelError("AuctionId", "Auction not found!");
            }
            else if (auction.CurrentPrice >= lbid.Amount)
            {
                ModelState.AddModelError("Amount", "Bid amount must exceed current bid");
            }
            else
            {

                if (auction.CurrentPrice < auction.productPrice)
                {
                    lbid.Username = User.Identity.Name;
                    auction.LiveBids.Add(lbid);
                    auction.status = "pending";
                    auction.countLiveBid = 0;
                    auction.CurrentPrice = lbid.Amount;
                    db.SaveChanges();
                }
                else if (auction.CurrentPrice == auction.productPrice)
                {
                    DateTime startTime = DateTime.Now;
                    DateTime endTime = DateTime.Now.AddSeconds(30);

                    lbid.Username = User.Identity.Name;
                    auction.LiveBids.Add(lbid);
                    auction.countLiveBid = 0;
                    auction.EndTime = endTime;
                    auction.status = "counting started!";
                    auction.CurrentPrice = lbid.Amount;
                    db.SaveChanges();
                }
                else if (auction.CurrentPrice > auction.productPrice)
                {

                    DateTime startTime = DateTime.Now;
                    DateTime endTime = DateTime.Now.AddSeconds(30);

                    if (auction.countLiveBid == 0 && auction.EndTime > startTime)
                    {
                        lbid.Username = User.Identity.Name;
                        auction.LiveBids.Add(lbid);
                        auction.countLiveBid = 1;
                        auction.EndTime = endTime;
                        auction.status = "1st Bid has been counting";
                        auction.CurrentPrice = lbid.Amount;
                        db.SaveChanges();
                    }
                    else if (auction.countLiveBid == 1 && auction.EndTime > startTime)
                    {
                        lbid.Username = User.Identity.Name;
                        auction.LiveBids.Add(lbid);
                        auction.countLiveBid = 2;
                        auction.status = "2nd Bid has been counting";
                        auction.CurrentPrice = lbid.Amount;
                        db.SaveChanges();
                    }
                    else if (auction.countLiveBid == 2 && auction.EndTime > startTime)
                    {
                        lbid.Username = User.Identity.Name;
                        auction.LiveBids.Add(lbid);
                        auction.countLiveBid = 0;
                        auction.status = "3rd Bid  has been counting";
                        auction.CurrentPrice = lbid.Amount;
                        db.SaveChanges();
                    }

                    else
                    {
                        TempData["status"] = "Sorry, the bid has expired! and Auction has Finilized!";
                        var su = auction.LiveBids.Last(c => c.LiveAuctionId == lbid.LiveAuctionId);
                        TempData["Winner"] = su.Username;
                        return RedirectToAction("LiveAuction", new { id = lbid.LiveAuctionId });
                    }
                }
            }
            if (!Request.IsAjaxRequest())
            {
                return RedirectToAction("LiveAuction", new { id = lbid.LiveAuctionId });
            }
            else
            {
                var httpStatus = ModelState.IsValid ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
                return new HttpStatusCodeResult(httpStatus);
            }
        }

        [Authorize]
        public ActionResult LiveBidHistory()
        {
            var id = TempData["id"];
            AuctionsDataContext DB = new AuctionsDataContext();
            var result = DB.LiveBids.Where(c => c.LiveAuctionId.ToString() == id.ToString());
            return View(result.ToList());
        }



        [Authorize]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LiveAuction liveAuction = db.LiveAuctions.Find(id);
            if (liveAuction == null)
            {
                return HttpNotFound();
            }
            return View(liveAuction);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,productName,productPrice,productType,productAddress,StartTime,EndTime,CurrentPrice,ImageURL,SellerUserID,status,countLiveBid")] LiveAuction liveAuction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(liveAuction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(liveAuction);
        }

        [Authorize]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LiveAuction liveAuction = db.LiveAuctions.Find(id);
            if (liveAuction == null)
            {
                return HttpNotFound();
            }
            return View(liveAuction);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(long id)
        {
            LiveAuction liveAuction = db.LiveAuctions.Find(id);
            db.LiveAuctions.Remove(liveAuction);
            db.SaveChanges();
            return RedirectToAction("Index");
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
