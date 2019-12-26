using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using carpoolapp.Models;
using carpoolapp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace carpoolapp.Controllers
{
    public class RegistrationController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout()
        {
            int userId = int.Parse(HttpContext.Session.GetString("ConnectedUserId"));

            using (CarpoolDbContext db = new CarpoolDbContext())
            {
                string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                string sqlQuery = $"UPDATE Credentials SET LastLoggedOut = '{date}' WHERE UserId = {userId};";
                
                db.Database.ExecuteSqlCommand(sqlQuery);
            }
            
            HttpContext.Session.SetString("ConnectedUserId", "");
            HttpContext.Session.SetString("ConnectedUsername", "");

            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult Login(Credentials credentials)
        {
            Debug.WriteLine("App User username => ", credentials.Username);
            Debug.WriteLine("App User password => ", credentials.UserPassword);
            

            string sqlQuery = $"SELECT Username, ";
            
            using (CarpoolDbContext db = new CarpoolDbContext())
            {
                // Get all credentials records
                List<Credentials> users = db.Credentials.FromSql($"SELECT * FROM Credentials;").ToList();
                
                foreach (var user in users)
                {
                    // If user exists
                    if(string.Compare(user.Username, credentials.Username) == 0 && string.Compare(user.UserPassword, credentials.UserPassword) == 0)
                    {
                        HttpContext.Session.SetString("ConnectedUserId", user.UserId.ToString());
                        HttpContext.Session.SetString("ConnectedUsername", user.Username);

                        string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        int userId = int.Parse(HttpContext.Session.GetString("ConnectedUserId"));

                        // Set the LastLoggedin column
                        string updateQuery = $"UPDATE Credentials SET LastLoggedin = '{date}' WHERE UserId = {userId};";
                        db.Database.ExecuteSqlCommand(updateQuery);


                        return RedirectToAction("Index", "Home");
                    }
                }

                // If user does not exist
                ViewBag.Message = "Username or Password is incorrect";
                return View();
            }
        }
        
        public IActionResult Register()
        {
            ViewBag.Message = TempData["ErrorMessage"];

            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterUserViewModel registerUserViewModel)
        {
            var newCredentials = registerUserViewModel.newCredentials;
           
            Credentials credentials = new Credentials
            {
                Username = newCredentials.Username,
                UserPassword = newCredentials.UserPassword
            };

            using (CarpoolDbContext db = new CarpoolDbContext() )
            {
                // Get all credentials records
                List<Credentials> users = db.Credentials.FromSql($"SELECT * FROM Credentials;").ToList();


                foreach (var user in users)
                {
                    // If user exists
                    if (string.Compare(user.Username, credentials.Username) == 0)
                    {

                        // Return error message
                        TempData["ErrorMessage"] = "Username already exists";
                        return RedirectToAction("Register");
                    }
                }



                var newAppUser = registerUserViewModel.newAppUser;
                
                var credentialsQuery = $"INSERT INTO Credentials (Username, UserPassword) VALUES ({newCredentials.Username}, {newCredentials.UserPassword})";
                
                db.Database.ExecuteSqlCommand($"EXECUTE spProcedureCreateNewUser {credentials.Username}, {credentials.UserPassword}, {newAppUser.FirstName}, {newAppUser.LastName}, {newAppUser.PhoneNumber}, {newAppUser.EmailAddress}");
            }
            
            return RedirectToAction("Login");
        }


    }
}