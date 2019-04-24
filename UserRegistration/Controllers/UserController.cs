using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserRegistration.Models;
using System.Net;
using System.Net.Mail;


namespace UserRegistration.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            // Fetching the Yes, No properties fro WorkinngDetails table
            MyDBEntities db = new MyDBEntities();
            List<WorkingDetail> list = db.WorkingDetails.ToList();
            ViewBag.WorkingDetail = new SelectList(list, "IsWorkingID", "IsWorking");
            return View();
        }


        [HttpPost]
        public ActionResult SaveRecord(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }
            else
            {
                try
                {
                    //Storing the forms values into User table of Database
                    MyDBEntities db = new MyDBEntities();
                    User userObj = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        PhoneNo = model.PhoneNo,
                        IsWorkingID = model.IsWorkingID
                    };
                    db.Users.Add(userObj);
                    db.SaveChanges();

                    //This function is use to send the email to the user
                    MailMessage mm = new MailMessage("mrnoumanbaloch@gmail.com", model.Email)
                    {
                        Subject = "Email from MTBC",
                        Body = "Hello dear " + model.FirstName + Environment.NewLine + "Many thanks for your order." + Environment.NewLine + "We will deliver the goods in less than 48 hours." + Environment.NewLine + "Kind Regards," + Environment.NewLine + "Jennifer" + Environment.NewLine + Environment.NewLine + "Please visit our website"
                    };
                    ;
                    mm.IsBodyHtml = false;
                    SmtpClient smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true
                    };
                    NetworkCredential nc = new NetworkCredential("mrnoumanbaloch@gmail.com", "Password");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = nc;
                    smtp.Send(mm);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}