using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using iHubAdminAPI.Models;
using iHubAdminAPI.Mailer;
using System.Web.Configuration;

namespace iHubAdminAPI.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }
            if (user.OnHold)
            {
                context.SetError("on_hold", "Your account is on hold. Please contact admin.");
                return;
            }
            var PasswordExpiryLimitInDays = WebConfigurationManager.AppSettings["PasswordExpiryLimitInDays"];
            //var PasswordExpiryLimitInDays = 30; // 2 years.
            if (user.Last_Updated_Datetime.AddDays(Convert.ToInt32(PasswordExpiryLimitInDays)) <= DateTime.Now)
            {
                context.SetError("password_expiry", "Your password has expired. Please reset your password.");
                //properties = CreateProperties(user.UserName, user.PhoneNumber, user.Id.ToString(), roles.FirstOrDefault().ToString(), user.StoreID.ToString(), user.Unit_Id.ToString(), OTP, user.isExpired);
                return;

            }
            string OTP = "";
            if (user.TwoFactorEnabled)
            {
                OTP = Common.GenerateOTP();
                string Message = string.Empty;
                var Currenttimeanddate = System.DateTime.Now.ToString();
                //Message = "OTP for " + user.UserName + ":" + OTP + "  Initiated Time:" + Currenttimeanddate ;
                //Common.sendmessage(user.PhoneNumber, Message, "1007161613703028192");
                Common.sendAdminOTP(user.Id, user.PhoneNumber, "Verification");
                IUsermailer mailer = new Usermailer();
                mailer.SuperAdminMail(user.Email, user.UserName, OTP).Send();
            }

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
               OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
                CookieAuthenticationDefaults.AuthenticationType);
            
            var roles = userManager.GetRolesAsync(user.Id).Result;
            oAuthIdentity.AddClaim(new Claim("RoleName", roles.FirstOrDefault() ?? ""));
            AuthenticationProperties properties = CreateProperties(user.UserName, user.PhoneNumber, user.Id.ToString(), roles.FirstOrDefault().ToString(), user.StoreID.ToString(), user.Unit_Id.ToString(), OTP);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        public override async Task GrantCustomExtension(OAuthGrantCustomExtensionContext context)
        {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
                if (context.GrantType != "expiry")
                {
                    context.SetError("invalid_grant", "unsupported grant_type");
                    return;
                }


                var userName = context.Parameters.Get("username");
                if (userName == null)
                {
                    context.SetError("invalid_grant", "username is required");
                    return;
                }
                var password = context.Parameters.Get("password");
                if (password == null)
                {
                    context.SetError("invalid_grant", "password is required");
                    return;
                }

                var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

                ApplicationUser user = await userManager.FindAsync(userName, password);

                if (user == null)
                {
                    context.SetError("old_password", "Username or Old password is incorrect.");
                    return;
                }

                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
                  OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
                    CookieAuthenticationDefaults.AuthenticationType);
                string OTP = "";
                var roles = userManager.GetRolesAsync(user.Id).Result;
                oAuthIdentity.AddClaim(new Claim("RoleName", roles.FirstOrDefault() ?? ""));
                AuthenticationProperties properties = CreateProperties(user.UserName, user.PhoneNumber, user.Id.ToString(), roles.FirstOrDefault().ToString(), user.StoreID.ToString(), user.Unit_Id.ToString(), OTP);
                AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
                context.Validated(ticket);
                context.Request.Context.Authentication.SignIn(cookiesIdentity);
            }
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName, string PhoneNumber, string id, string role, string userid, string unitid, string OTP)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName },
                { "phoneNumber", PhoneNumber },
                {"id",id },
                { "OTP",OTP},
                { "role",role},
                { "userid",userid},
                { "unitid",unitid}

            };
            return new AuthenticationProperties(data);
        }
    }
}