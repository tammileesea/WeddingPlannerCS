using System.Collections.Generic;

namespace WeddingPlanner.Models {
    public class DashboardViewModel {
        public User LoggedInUser {get;set;}
        public List<Wedding> EveryWedding {get;set;}
        public Wedding OneWedding {get;set;}
        public WeddingGuest AttendingUser {get;set;}

        public bool IsAttending {get;set;}
    }
}