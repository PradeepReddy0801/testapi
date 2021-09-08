using Mvc.Mailer;
using System.Configuration;

using System.Net.Mail;
using System;
using System.Collections.Generic;

using System.Linq;

using System.Web;
using iHubAdminAPI.Models;

namespace iHubAdminAPI.Mailer
{
    public class Usermailer : MailerBase, IUsermailer
    {
        public virtual MvcMailMessage Welcome(string Email)
        {

            return Populate(x =>
            {
                x.Subject = "WelCome To iHub";
                x.ViewName = "WelCome";
                x.To.Add(Email);
            });

        }

        public virtual MvcMailMessage WebAdminMail(string Email, string storeid)
        {
            ViewBag.StoreID = storeid;
            return Populate(x =>
            {
                x.Subject = "WelCome To iHub";
                x.ViewName = "webadminmail";
                x.To.Add(Email);
            });
        }

        public virtual MvcMailMessage franchisemail(string Email, string storeid)
        {
            ViewBag.StoreID = storeid;
            return Populate(x =>
            {
                x.Subject = "Welcome To IHUB";
                x.ViewName = "franchisemail";
                x.To.Add(Email);
            });
        }
        public virtual MvcMailMessage storeasistance(string Email, string storeid)
        {
            ViewBag.StoreID = storeid;
            return Populate(x =>
            {
                x.Subject = "Store Assistant Login ID and Password";
                x.ViewName = "storeasistance";
                // x.Attachments.Add("test.docx", System.IO.File.ReadAllBytes(docPath));
                x.To.Add(Email);
            });
        }

        public virtual MvcMailMessage warehouseLowStock(string docPath, string warehouseName)
        {
            string Email = ConfigurationManager.AppSettings["warehouseMailids1"].ToString() + "," +
                           ConfigurationManager.AppSettings["warehouseMailids2"].ToString() + "," +
                           ConfigurationManager.AppSettings["warehouseMailids3"].ToString();
            return Populate(x =>
            {
                x.Subject = warehouseName + " Low Stock Report on " + DateTime.Now.ToString("dd-MMM-yy");
                x.ViewName = "warehouseLowStock";
                x.Attachments.Add(new Attachment(docPath));
                x.To.Add(Email);
            });
        }


        public virtual MvcMailMessage SuperAdminMail(string Email, string Role, string OTP)
        {
            var Currenttimeanddate = System.DateTime.Now.ToString();
            ViewBag.Role = Role;
            ViewBag.OTP = OTP;
            ViewBag.issued = Currenttimeanddate;
            return Populate(x =>
            {
                x.Subject = "OTP Login Authentication";
                x.ViewName = "superadminmail";
                x.To.Add(Email);
            });
        }
        public virtual MvcMailMessage ChangeBuyerOrderStatus(VMPagingResultsPost vmodal, string email)
        {
            ViewBag.OrderDetails = vmodal;

            return Populate(x =>
            {
                x.Subject = vmodal.Subject;
                x.ViewName = "BuyerOrderStatusmail";
                x.To.Add(email);
            });
        }

        public MvcMailMessage BuyerCancelOrder(VMOrdersModel ordersmodel)
        {
            throw new NotImplementedException();
        }

        public MvcMailMessage BuyerOrderStatusmail(VMPagingResultsPost vmodal, string email)
        {
            throw new NotImplementedException();
        }
    }
}