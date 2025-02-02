using BidZ.lk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BidZ.lk.Controllers
{
    public class HomeController : Controller
    {
        // normal biding search starting
        [HttpGet]
        public ActionResult Index()
        {
            string userid = User.Identity.GetUserId();
            var db = new ApplicationDbContext();
            if (userid == null)
            {
                ViewBag.userID = "NULL";
                AuctionsDataContext DB = new AuctionsDataContext();
                var protypeList = new SelectList(new[] { "jewellery", "Automobile", "RealEstate" });
                ViewBag.ProtypeList = protypeList;
                var dk = DB.Auctions.ToList();
                return View(dk);
            }
            else
            {
                var value = db.Users.Find(User.Identity.GetUserId());
                string getUserType = value.userType;
                ViewBag.userID = getUserType;
                Session["UserType"] = getUserType;
                var protypeList = new SelectList(new[] { "jewellery", "Automobile", "RealEstate" });
                ViewBag.ProtypeList = protypeList;
                AuctionsDataContext DB = new AuctionsDataContext();
                return View(DB.Auctions.ToList());
            }
        }


        [HttpGet]
        public ActionResult IndexDropResults(string dropdownResults)
        {
            AuctionsDataContext db = new AuctionsDataContext();
            var result = db.Auctions.Where(c => c.productType.StartsWith(dropdownResults));
            return View(result.ToList());
        }

        public ActionResult SearchResults(string dropdownResults)
        {
            AuctionsDataContext db = new AuctionsDataContext();
            var result = db.Auctions.Where(c => c.Title.StartsWith(dropdownResults));
            return View(result.ToList());
        }



        public ActionResult LiveBidIndexSearch(string dropdownResults)
        {
            AuctionsDataContext db = new AuctionsDataContext();
            var result = db.LiveAuctions.Where(c => c.productType.StartsWith(dropdownResults));
            return View(result.ToList());
        }



        public ActionResult LiveSearchBid(string dropdownResults)
        {
            AuctionsDataContext db = new AuctionsDataContext();
            var result = db.LiveAuctions.Where(c => c.Title.StartsWith(dropdownResults));
            return View(result.ToList());
        }


        //Live bidning search Finishing

        public ActionResult About()
        {
            ViewBag.Message = "The page has created for Localization and Internalization Auction targets. Three people are founders of this site (Thulanjana, Kavishka and Thanuka).";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Our contact Number -011-90392039";
            return View();
        }
    }
}