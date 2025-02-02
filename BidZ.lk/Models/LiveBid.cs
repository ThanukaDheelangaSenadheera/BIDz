using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BidZ.lk.Models
{
    public class LiveBid
    {
        public long Id { get; internal set; }

        public long LiveAuctionId { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        public string Username { get; set; }

        [Range(1, double.MaxValue)]
        public decimal Amount { get; set; }

        public LiveBid()
        {
            Timestamp = DateTime.Now;
        }

    }
}