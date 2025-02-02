using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BidZ.lk.Models
{
    public class Messages
    {
        [Key]
        [Required]
        public long Id { get; set; }


        [Required]
        [Display(Name = "Sender's Useremail")]
        public string SenderUserEmail { get; set; }

        [Required]
        [Display(Name = "Reciever's Username")]
        public string RecieverUsername { get; set; }

        [Required]
        [Display(Name = "Message Contents")]
        public string Contents { get; set; }


    }
}