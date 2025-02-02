using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BidZ.lk.Models
{
    public class AuctionsDataContext : DbContext
    {
        public DbSet<Auction> Auctions { get; set; }

        static AuctionsDataContext()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<AuctionsDataContext>());
        }
        public System.Data.Entity.DbSet<BidZ.lk.Models.Bid> Bids { get; set; }
        public System.Data.Entity.DbSet<BidZ.lk.Models.Messages> Message { get; set; }
        public System.Data.Entity.DbSet<BidZ.lk.Models.AutomaticBidModel> AutomaticBidModel { get; set; }

        public System.Data.Entity.DbSet<BidZ.lk.Models.LiveAuction> LiveAuctions { get; set; }

        public System.Data.Entity.DbSet<BidZ.lk.Models.LiveBid> LiveBids { get; set; }
    }
}
