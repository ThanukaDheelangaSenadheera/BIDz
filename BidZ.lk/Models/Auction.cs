using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace BidZ.lk.Models
{
    public class Auction
    {
        [Key]
        [Required]
        public long Id { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string productName { get; set; }

        [Required]
        [Display(Name = "Reserved Price")]
        public decimal productPrice { get; set; }

      
        [Required]
        [Display(Name = "Product Type")]
        public string productType { get; set; }

        [Display(Name = "Location")]
        public string productAddress { get; set; }

        [Required]
        [Display(Name = "Auction Start Time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "Auction End Time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndTime { get; set; }

        [Required]
        [DataType(DataType.Currency)]

        [Display(Name = "Auction Start Price")]
        public decimal StartPrice { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Auction Current Price")]
        public decimal? CurrentPrice { get; set; }
        
        
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Product Image URL")]
        public string ImageURL { get; set; }
        

        [Required]
        public string SellerUserID { get; set; }


        public virtual Collection<Bid> Bids { get; private set; }



        public int BidCount
        {
            get { return Bids.Count; }
        }


        public Auction()
        {
            Bids = new Collection<Bid>();
            CurrentPrice = 0;
        }
    }
}
