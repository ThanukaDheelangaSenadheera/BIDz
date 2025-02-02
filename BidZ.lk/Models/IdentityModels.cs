using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace BidZ.lk.Models
{
       public class ApplicationUser : IdentityUser
        {
        [Display(Name = "User First Name")]
        public string userFname { get; set; }

        [Display(Name = "User Last Name")]
        public string userLname { get; set; }

        [Display(Name = "User's Address")]
        public string address { get; set; }

        [Display(Name = "User's Contact Number")]
        public string userContactNo { get; set; }

        [Display(Name = "User's Commercial Name")]
        public string commercialName { get; set; }

        [Display(Name = "User's Commercial Address")]
        public string commercialAddress { get; set; }

        [Display(Name = "User's Commercial Email")]
        public string commercialEmail { get; set; }

        [Display(Name = "User Type")]
        public string userType { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<BidZ.lk.Models.RegisterViewModel> RegisterViewModels { get; set; }

    }
}