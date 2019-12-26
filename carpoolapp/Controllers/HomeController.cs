using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using carpoolapp.Models;
using Microsoft.AspNetCore.Http;
using carpoolapp.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace carpoolapp.Controllers
{

    public class HomeController : Controller
    {
        

        public IActionResult Index()
        {
            int userId = int.Parse(HttpContext.Session.GetString("ConnectedUserId"));

            using (CarpoolDbContext db = new CarpoolDbContext())
            {
                CarpoolsViewModel carpoolsViewModel = new CarpoolsViewModel();

                // Call procedure from database stored procedures and pass it the user id as parameter
                carpoolsViewModel.TripsList = db.Trip.FromSql("EXECUTE dbo.prGetCarpoolsNotOfUser {0}", userId).ToList();
                carpoolsViewModel.AppUserList = db.AppUser.FromSql("EXECUTE spProcedureGetCarpoolsNotOfUsersDetailedInformation {0}", userId).ToList();
                carpoolsViewModel.CommentsList = db.Comment.FromSql("SELECT * FROM Comment").ToList();
                carpoolsViewModel.CommentsTripsList = db.CommentsTrips.FromSql("EXECUTE prGetDetailedCommentsInformation").ToList();
                carpoolsViewModel.AppUser = db.AppUser.FromSql($"SELECT * FROM AppUser WHERE UserId = {userId};").ToList()[0];

                ViewData["connectedUsername"] = HttpContext.Session.GetString("ConnectedUsername");
                return View(carpoolsViewModel);
            }
            
        }


        public IActionResult Memberships()
        {
            int userId = int.Parse(HttpContext.Session.GetString("ConnectedUserId"));

            using (CarpoolDbContext db = new CarpoolDbContext())
            {
                CarpoolsViewModel carpoolsViewModel = new CarpoolsViewModel();

                carpoolsViewModel.CommentsList = db.Comment.FromSql("SELECT * FROM Comment").ToList();
                carpoolsViewModel.AppUserList = db.AppUser.FromSql("EXECUTE spProcedureGetCarpoolsNotOfUsersDetailedInformation {0}", userId).ToList();
                carpoolsViewModel.CommentsTripsList = db.CommentsTrips.FromSql("EXECUTE prGetDetailedCommentsInformation").ToList();
                carpoolsViewModel.TripsList = db.Trip.FromSql("EXECUTE dbo.spBookingsUser {0}", userId).ToList();
                carpoolsViewModel.AppUser = db.AppUser.FromSql($"SELECT * FROM AppUser WHERE UserId = {userId};").ToList()[0];

                ViewData["connectedUsername"] = HttpContext.Session.GetString("ConnectedUsername");
                return View(carpoolsViewModel);
            }

        }


        public IActionResult MyCarpools()
        {
            using (CarpoolDbContext db = new CarpoolDbContext())
            {
                //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                CarpoolsViewModel carpoolsViewModel = new CarpoolsViewModel();

                int userId = int.Parse(HttpContext.Session.GetString("ConnectedUserId"));
               
                // Get trips created by the current user order by their "created" time
                string sqlQuery = $"SELECT TripId, UserId, MeetingPlace, Departure, DepartureDate, DepartureTime, Destination, Price, SmokeFree, " +
                   $"Rating, NbrOfSeatsOpen, NbrOfMaxSeats, Date_Created FROM Trip WHERE UserId = {userId} ORDER BY Date_Created DESC;";


                carpoolsViewModel.TripsList = db.Trip.FromSql(sqlQuery).ToList(); 
                carpoolsViewModel.CommentsList = db.Comment.FromSql("SELECT * FROM Comment").ToList();
                carpoolsViewModel.CommentsTripsList = db.CommentsTrips.FromSql("EXECUTE prGetDetailedCommentsInformation").ToList();
                carpoolsViewModel.BookingsDetailsTripsList = db.BookingsDetailsTrips.FromSql("Execute spGetMembershipsTrip {0}", userId).ToList();
                carpoolsViewModel.AppUser = db.AppUser.FromSql($"SELECT * FROM AppUser WHERE UserId = {userId};").ToList()[0];


                //carpoolsViewModel.BookingsList = db.Booking.ToList();
                ViewData["connectedUsername"] = HttpContext.Session.GetString("ConnectedUsername");
                return View(carpoolsViewModel);
            }
        }


        public IActionResult CreateCarpool()
        {
            int userId = int.Parse(HttpContext.Session.GetString("ConnectedUserId"));

            using (CarpoolDbContext db = new CarpoolDbContext())
            {
                CarpoolsAddViewModel carpoolsAddViewModel = new CarpoolsAddViewModel();
                carpoolsAddViewModel.AppUser = db.AppUser.FromSql($"SELECT * FROM AppUser WHERE UserId = {userId};").ToList()[0];

                ViewData["connectedUsername"] = HttpContext.Session.GetString("ConnectedUsername");
                return View(carpoolsAddViewModel);
            }
        }


        [HttpPost]
        public IActionResult CreateCarpool(Trip newTrip)
        {
            int userId = int.Parse(HttpContext.Session.GetString("ConnectedUserId"));
           
            using (CarpoolDbContext db = new CarpoolDbContext())
            {
                DateTime d = Convert.ToDateTime(newTrip.DepartureDate);
                string parsedDate = $"{d.Year}-{d.Month}-{d.Day}";

                string sqlQuery = $"INSERT INTO Trip(UserId, MeetingPlace, Departure, DepartureDate, DepartureTime, Destination, Price, SmokeFree, " +
                    $"Rating, NbrOfSeatsOpen, NbrOfMaxSeats) VALUES({userId}, '{newTrip.MeetingPlace}', '{newTrip.Departure}', " +
                    $"'{parsedDate}', '{newTrip.DepartureTime.ToString()}', '{newTrip.Destination}', '{newTrip.Price}', '{newTrip.SmokeFree}', 0, {newTrip.NbrOfMaxSeats}, {newTrip.NbrOfMaxSeats});";
                

                db.Database.ExecuteSqlCommand(sqlQuery);

                ViewData["connectedUsername"] = HttpContext.Session.GetString("ConnectedUsername");
                // Redirect the user to his carpools personal page
                return RedirectToAction("MyCarpools");
            }
           
        }

        public IActionResult JoinCarpool(int id)
        {
            using (CarpoolDbContext db = new CarpoolDbContext())
            {
                int userId = int.Parse(HttpContext.Session.GetString("ConnectedUserId"));
                string sqlQuery = $"INSERT INTO Booking (TripId, UserId, BookStatus) Values ({id}, {userId}, 'Open');";
               
                db.Database.ExecuteSqlCommand(sqlQuery);
            }

            return RedirectToAction("Index");
        }

        
        public IActionResult MyCarpoolsDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (CarpoolDbContext db = new CarpoolDbContext())
            {
                var tripToDelete = db.Trip.SingleOrDefault(x => x.TripId == id);
                string sqlQuery = $"DELETE FROM Trip WHERE TripId = {id};";
                
                db.Database.ExecuteSqlCommand(sqlQuery);

                return RedirectToAction("MyCarpools");
            }

        }

        public IActionResult EditCarpool(int id)
        {
            int userId = int.Parse(HttpContext.Session.GetString("ConnectedUserId"));

            if (id == null)
            {
                return NotFound();
            }

            using (CarpoolDbContext db = new CarpoolDbContext())
            {
                CarpoolsEditViewModel carpoolsEditViewModel = new CarpoolsEditViewModel();
                carpoolsEditViewModel.AppUser = db.AppUser.FromSql($"SELECT * FROM AppUser WHERE UserId = {userId};").ToList()[0];


                string sqlQuery = $"SELECT * FROM Trip WHERE TripId = {id}";

                carpoolsEditViewModel.editTrip = db.Trip.FromSql(sqlQuery).ToList()[0];

                ViewData["connectedUsername"] = HttpContext.Session.GetString("ConnectedUsername");

                return View(carpoolsEditViewModel);
            }
        }

        [HttpPost]
        public IActionResult EditCarpool(CarpoolsEditViewModel carpoolsEditViewModel)
        {
            Trip trip = carpoolsEditViewModel.editTrip;
            DateTime d = Convert.ToDateTime(trip.DepartureDate);
            string parsedDate = $"{d.Year}-{d.Month}-{d.Day}";

            string sqlQuery = $"UPDATE Trip SET MeetingPlace = '{trip.MeetingPlace}'," +
                                              $"Departure = '{trip.Departure}'," +
                                              $"DepartureDate = '{parsedDate}'," +
                                              $"DepartureTime = '{trip.DepartureTime}'," +
                                              $"Destination = '{trip.Destination}'," +
                                              $"SmokeFree = '{trip.SmokeFree}'," +
                                              $"Price = {trip.Price} WHERE TripId = {trip.TripId}" ;
            
            using (CarpoolDbContext db = new CarpoolDbContext())
            {
                db.Database.ExecuteSqlCommand(sqlQuery);

                ViewData["connectedUsername"] = HttpContext.Session.GetString("ConnectedUsername");
                return RedirectToAction("MyCarpools");
            }

        }

        public IActionResult CancelMembership(int id)
        {
            int userId = int.Parse(HttpContext.Session.GetString("ConnectedUserId"));

            using (CarpoolDbContext db = new CarpoolDbContext())
            {
                CarpoolsViewModel carpoolsViewModel = new CarpoolsViewModel();

                string sqlQuery = $"DELETE FROM Booking WHERE TripId = {id} AND UserId = {userId};";

                db.Database.ExecuteSqlCommand(sqlQuery);

                ViewData["connectedUsername"] = HttpContext.Session.GetString("ConnectedUsername");
                return RedirectToAction("Memberships");
            }

        }


        [HttpPost]
        public IActionResult CreateComment(string commentText, string tripId)
        {
            int userId = int.Parse(HttpContext.Session.GetString("ConnectedUserId"));

            using (CarpoolDbContext db = new CarpoolDbContext())
            {
                string sqlQuery = $"INSERT INTO Comment (CommentText, TripId, UserId) VALUES('{commentText.ToString().Replace("'","''")}', {tripId}, {userId});";

                db.Database.ExecuteSqlCommand(sqlQuery);


                ViewData["connectedUsername"] = HttpContext.Session.GetString("ConnectedUsername");
                // Redirect the user to his carpools personal page
            }
            return RedirectToAction("Index");
        }



        public IActionResult UserProfile()
        {
            int userId = int.Parse(HttpContext.Session.GetString("ConnectedUserId"));

            string sqlQueryAppUser = $"SELECT * FROM AppUser WHERE UserId = {userId};";
            string sqlQueryCredentials = $"SELECT UserId, Username, UserPassword, LastLoggedin, LastLoggedOut FROM Credentials WHERE UserId = {userId};";
            ;
            using (CarpoolDbContext db = new CarpoolDbContext())
            {
                UserProfileViewModel userProfileViewModel = new UserProfileViewModel();

                userProfileViewModel.AppUser = db.AppUser.FromSql(sqlQueryAppUser).ToList()[0];
                userProfileViewModel.credentials = db.Credentials.FromSql(sqlQueryCredentials).ToList()[0];
                userProfileViewModel.analytics = db.UserAnalytics.FromSql("EXECUTE spGetNumberTripsOfUser {0}", userId).ToList()[0];



                ViewData["connectedUsername"] = HttpContext.Session.GetString("ConnectedUsername");
                ViewBag.UpdateMessage = TempData["UpdateMessage"];
                return View(userProfileViewModel);
            }
        }

       
        [HttpPost]
        public IActionResult EditProfile(string phone, string email, string password, IFormFile Image)
        {
            int userId = int.Parse(HttpContext.Session.GetString("ConnectedUserId"));
           

            using (CarpoolDbContext db = new CarpoolDbContext())
            {
               
                byte[] profilePicBinary = null;

                if (Image != null)
                {
                   
                    if (Image.Length > 0)
                    {
                        //Convert Image to byte and save to database
                        using (var fs1 = Image.OpenReadStream())
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            profilePicBinary = ms1.ToArray();
                            db.Database.ExecuteSqlCommand(@"UPDATE AppUser SET ProfilePicture = @profilePicBinary WHERE UserId = @userId", 
                                        new SqlParameter("profilePicBinary", profilePicBinary), 
                                        new SqlParameter("userId", userId));

                        }
                    }
                }
                string sqlQueryCredentials = $"Update Credentials SET UserPassword = '{password}' WHERE UserId = {userId};";
                string sqlQueryAppUser = $"Update AppUser SET PhoneNumber = '{phone}', EmailAddress = '{email}' WHERE UserId = {userId};";
                db.Database.ExecuteSqlCommand(sqlQueryCredentials);
                db.Database.ExecuteSqlCommand(sqlQueryAppUser);

            }

            ViewData["connectedUsername"] = HttpContext.Session.GetString("ConnectedUsername");
            TempData["UpdateMessage"] = "Your profile has been updated!";
            return RedirectToAction("UserProfile");
        }



        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
