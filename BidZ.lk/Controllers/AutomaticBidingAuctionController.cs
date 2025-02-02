using BidZ.lk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BidZ.lk.Controllers
{
    public class AutomaticBidingAuctionController : Controller
    {
        private AuctionsDataContext db = new AuctionsDataContext();
        private Bid bidModel = new Models.Bid();
        // GET: AutomaticBidingAuction
        public ActionResult Index()
        {
            return View();
        }        
        
        [HttpGet]
        public ActionResult AutomaticBidFeature(long id)
        {
            var db = new AuctionsDataContext();
            var auction = db.AutomaticBidModel.Find(id);           
            return View(auction);
        }      

        [HttpPost]
        public ActionResult Bid(AutomaticBidModel abm)
        {
            var db = new AuctionsDataContext();
            var auction = db.Auctions.Find(abm.bidId);
            if (auction == null)
            {
                ModelState.AddModelError("AuctionId", "Auction not found!");
                
            }
            else
            {
                //auction calculation end time...

                //while (true)
                //{
                //if this is null it gives an exception


                var jk = db.AutomaticBidModel.Find(abm.bidId);
                var lstbit = db.Bids.Find(abm.bidId);
                //db.AutomaticBidModel.Find(abm.bidId)
                if ((lstbit == null) && (jk.Equals(null)))
                {
                    //abm.username = abm.username;
                    //abm.HighOffer = abm.HighOffer;
                    //abm.bidId = abm.bidId;
                    //abm.IncrementValue = abm.IncrementValue;
                    //db.SaveChanges();
                    //return RedirectToAction("AutomaticBidFeature", new { id = abm.bidId });
                }
                else
                {
                    
                    var su = auction.Bids.Last(c => c.AuctionId == abm.bidId);
                    //if this is null it gives an exception
                    var automaticBidValues = db.AutomaticBidModel.Last(c => c.bidId == abm.bidId);



                    // retrieve values from the database
                    int highoffer = automaticBidValues.HighOffer;
                        int increment = automaticBidValues.IncrementValue;
                        decimal AutoCurrentPrice = su.Amount;
                        // you need to get the current price using on database
                                                
                        for (int x = 0; (highoffer >= x) && (su.Amount > AutoCurrentPrice); x += increment)
                        {
                            AutoCurrentPrice = AutoCurrentPrice + (decimal)increment;
                            //saving data would be change
                            abm.username = su.Username;
                            abm.HighOffer = highoffer;
                            abm.bidId = su.Id;
                            abm.IncrementValue = increment;
                            db.SaveChanges();
                            
                        //db.AutomaticBidModel.Add(auction);
                        }
                        //creating a new instances of a bid model and assign new values to the bid model
                        bidModel.Username = su.Username;
                        bidModel.Amount = AutoCurrentPrice;
                        bidModel.AuctionId = su.AuctionId;
                        bidModel.Timestamp = su.Timestamp;
                        auction.Bids.Add(bidModel);
                        auction.CurrentPrice = AutoCurrentPrice;
                        db.SaveChanges();
                        return RedirectToAction("AutomaticBidFeature", new { id = abm.bidId });
                 }
            }
            return RedirectToAction("", new { id = abm.bidId });

            //if (!Request.IsAjaxRequest())
            //{
            //    return RedirectToAction("AutomaticBidFeature", new { id = abm.bidId });
            //}
            //else
            //{
            //    var httpStatus = ModelState.IsValid ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            //    return new HttpStatusCodeResult(httpStatus);
            //}

        }
    }
}