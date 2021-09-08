using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Security.Principal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace iHubAdminAPI.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<int, UserLogin, UserRole, UserClaim>
    {
        public int StoreID { get; set; }
        public int Unit_Id { get; set; }
        public DateTime? ActiveUntil;
        public DateTime Last_Updated_Datetime { get; set; }
        public bool OnHold { get; set; }

        public ApplicationUser() : base()
        {
            //   PasswordHistories = new List<PasswordHistory>();
        }
        //public virtual IList<PasswordHistory> PasswordHistories { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager  manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("PhoneNumber", this.PhoneNumber));
            userIdentity.AddClaim(new Claim("UserId", this.Id.ToString()));
            userIdentity.AddClaim(new Claim("StoreId", this.StoreID.ToString()));
            userIdentity.AddClaim(new Claim("UnitID", this.Unit_Id.ToString()));
            return userIdentity;
        }

        public static implicit operator IdentityUser(ApplicationUser v)
        {
            throw new NotImplementedException();
        }
    }
    // New derived classes
    public class UserRole : IdentityUserRole<int>
    {
    }

    public class UserClaim : IdentityUserClaim<int>
    {
    }

    public class UserLogin : IdentityUserLogin<int>
    {
    }

    public class Role : IdentityRole<int, UserRole>
    {
        public Role() { }
        public Role(string name) { Name = name; }
    }

    public class ApplicationUserStore : UserStore<ApplicationUser, Role, int,
           UserLogin, UserRole, UserClaim>
    {
        public ApplicationUserStore(ApplicationDbContext context) : base(context)
        {
        }
        public override async Task CreateAsync(ApplicationUser user)
        {
            await base.CreateAsync(user);
            //  await AddToPreviousPasswordsAsync(user, user.PasswordHash);
        }
    }

    public class RoleStore : RoleStore<Role, int, UserRole>
    {
        public RoleStore(ApplicationDbContext context) : base(context)
        {
        }
    }
    public static class IdentityExtensions
    {


        public static string GetPhoneNumber(this IIdentity identity)
        {
            return ((ClaimsIdentity)identity).FindFirst("PhoneNumber").Value;
        }
        public static int GetUserID(this IIdentity identity)
        {
            return Convert.ToInt32(((ClaimsIdentity)identity).FindFirst("UserId").Value);
        }
        public static string GetRoleName(this IIdentity identity)
        {
            return ((ClaimsIdentity)identity).FindFirst("RoleName").Value;
        }
        public static string GetRoleID(this IIdentity identity)
        {
            return ((ClaimsIdentity)identity).FindFirst("RoleID").Value;
        }
        public static string GetStoreID(this IIdentity identity)
        {
            return ((ClaimsIdentity)identity).FindFirst("StoreId").Value;
        }
        public static string GetUnitID(this IIdentity identity)
        {
            return ((ClaimsIdentity)identity).FindFirst("UnitID").Value;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role, int,
    UserLogin, UserRole, UserClaim>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}