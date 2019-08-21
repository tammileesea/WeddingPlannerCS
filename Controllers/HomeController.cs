using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Models;

namespace WeddingPlanner.Controllers
{
    public class HomeController : Controller
    {
        private WeddingPlannerContext dbContext;
        public HomeController(WeddingPlannerContext context){
            dbContext = context; 
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(LoginRegViewModel modelData) {
            User creatingUser = modelData.newUser;
            if(ModelState.IsValid){
                if(dbContext.users.Any(u => u.Email == creatingUser.Email)){
                    ModelState.AddModelError("Email", "Email is already in use!");
                    return View("Index");
                } else {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    creatingUser.Password = Hasher.HashPassword(creatingUser, creatingUser.Password);
                    dbContext.Add(creatingUser);
                    dbContext.SaveChanges();
                    if(HttpContext.Session.GetInt32("UserId") == null){
                        HttpContext.Session.SetInt32("UserId", creatingUser.UserId);
                    }
                    return RedirectToAction("Dashboard", new {userID = creatingUser.UserId});
                }
            } else {
                System.Console.WriteLine("*******************");
                System.Console.WriteLine("REGISTRATION NOT WORKING!!!!");
                System.Console.WriteLine(creatingUser.FirstName);
                System.Console.WriteLine(creatingUser.LastName);
                System.Console.WriteLine(creatingUser.Email);
                System.Console.WriteLine("*******************");
                return View("Index");
            }
        }

        public IActionResult Login(LoginRegViewModel modelData){
            LoginUser userLogin = modelData.existingUser;
            if(ModelState.IsValid){
                User userInDB = dbContext.users.FirstOrDefault(u => u.Email == userLogin.Email);
                if(userInDB == null){
                    ModelState.AddModelError("Email", "Invalid email or password");
                    return View("Index");
                } else {
                    var hasher = new PasswordHasher<LoginUser>();
                    var result = hasher.VerifyHashedPassword(userLogin, userInDB.Password, userLogin.Password);
                    if(result == 0){
                        ModelState.AddModelError("Password", "Invalid email or password");
                        return View("Index");
                    }
                    if(HttpContext.Session.GetInt32("UserId") == null){
                        HttpContext.Session.SetInt32("UserId", userInDB.UserId);
                    }
                    return RedirectToAction("Dashboard", new {userID = userInDB.UserId});
                }
            } else {
                return View("Index");
            }
        }

        [HttpGet("dashboard/{UserID}")]
        public IActionResult Dashboard(int UserID) {
            if(HttpContext.Session.GetInt32("UserId") == null){
                return RedirectToAction("Index");
            }
            DashboardViewModel dashModel = new DashboardViewModel();
            dashModel.LoggedInUser = dbContext.users.SingleOrDefault(u => u.UserId == UserID);
            dashModel.EveryWedding = dbContext.weddings.Include(w => w.Guests).ThenInclude(g => g.Marriage).OrderBy(w => w.WeddingDate).ToList();
            return View(dashModel);
        }

        [HttpGet("{UserID}/createWedding")]
        public IActionResult CreateWedding(int UserID){
            if(HttpContext.Session.GetInt32("UserId") == null){
                return RedirectToAction("Index");
            }
            DashboardViewModel dashModel = new DashboardViewModel();
            dashModel.LoggedInUser = dbContext.users.SingleOrDefault(u => u.UserId == UserID);
            return View(dashModel);
        }

        [HttpPost("{UserID}/AddWeddingToDB")]
        public IActionResult AddWeddingToDB(DashboardViewModel modelData, int UserID) {
            Wedding CreatingWedding = modelData.OneWedding;
            System.Console.WriteLine("**************************");
            System.Console.WriteLine(ModelState.IsValid);
            System.Console.WriteLine("**************************");
            if (ModelState.IsValid) {
                CreatingWedding.CreatorId = UserID;
                dbContext.Add(CreatingWedding);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard", new {UserID = UserID});
            }
            DashboardViewModel dashModel = new DashboardViewModel();
            dashModel.LoggedInUser = dbContext.users.SingleOrDefault(u => u.UserId == UserID);
            dashModel.EveryWedding = dbContext.weddings.Include(w => w.Guests).ThenInclude(g => g.Marriage).ToList();
            return View("CreateWedding", dashModel);
        }

        [HttpGet("{userID}/weddingInfo/{weddingID}")]
        public IActionResult WeddingInfo(int userID, int weddingID) {
            if(HttpContext.Session.GetInt32("UserId") == null){
                return RedirectToAction("Index");
            }
            DashboardViewModel dashModel = new DashboardViewModel();
            dashModel.LoggedInUser = dbContext.users.SingleOrDefault(u => u.UserId == userID);
            dashModel.EveryWedding = dbContext.weddings.Include(w => w.Guests).ThenInclude(g => g.Marriage).ToList();
            dashModel.OneWedding = dbContext.weddings.Include(w => w.Guests).ThenInclude(g => g.Attendant).SingleOrDefault(w => w.WeddingId == weddingID);
            return View(dashModel);
        }

        [HttpPost("{userID}/removeWedding/{weddingID}")]
        public IActionResult RemoveWedding(int userID, int weddingID){
            Wedding WeddingToRemove = dbContext.weddings.SingleOrDefault(w => w.WeddingId == weddingID);
            dbContext.weddings.Remove(WeddingToRemove);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard", new { UserID = userID});
        }

        [HttpPost("{userID}/RSVPWedding/{weddingID}")]
        public IActionResult RSVPWedding(int userID, int weddingID) {
            WeddingGuest AddGuest = new WeddingGuest();
            AddGuest.AttendantId = userID;
            AddGuest.MarriageId = weddingID;
            dbContext.Add(AddGuest);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard", new { UserID = userID});
        }

        [HttpPost("{userID}/UnableAttendWedding/{weddingID}")]
        public IActionResult UnableAttendWedding(int userID, int weddingID) {
            WeddingGuest findGuest = dbContext.weddingGuests.SingleOrDefault(g => g.AttendantId == userID && g.MarriageId == weddingID);
            dbContext.weddingGuests.Remove(findGuest);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard", new { UserID = userID});
        }

        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
