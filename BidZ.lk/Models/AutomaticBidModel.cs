using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BidZ.lk.Models
{
    public class AutomaticBidModel
    {

        [Key]
        [Required]
        public long Id { get; set; }
        
        public long bidId { get; set; }
        [Required]
        [Display(Name = "Username")]
        public string username { get; set;}
        
        [Required]
        [Display(Name = "High Offer Value")]
        public int HighOffer { get; set; }
        
        [Required]
        [Display(Name = "Increment Value")]
        public int IncrementValue { get; set; }

    }
}