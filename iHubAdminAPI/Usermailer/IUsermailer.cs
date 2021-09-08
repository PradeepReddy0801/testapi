using iHubAdminAPI.Models;
using Mvc.Mailer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iHubAdminAPI.Mailer
{
	public interface IUsermailer
	{
        MvcMailMessage Welcome(string Email);
        MvcMailMessage WebAdminMail(string Email,string storeid);
        MvcMailMessage franchisemail(string Email, string storeid);
        MvcMailMessage storeasistance(string Email, string storeid);
        MvcMailMessage SuperAdminMail(string Email, string Role, string OTP);
        MvcMailMessage warehouseLowStock(string docPath, string warehouseName);
        MvcMailMessage ChangeBuyerOrderStatus(VMPagingResultsPost vmodal, string email);
        MvcMailMessage BuyerCancelOrder(VMOrdersModel ordersmodel);
        MvcMailMessage BuyerOrderStatusmail(VMPagingResultsPost vmodal, string email);
    }
}