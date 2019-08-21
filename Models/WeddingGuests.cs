using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models {
    public class WeddingGuest {
        [Key]
        public int WeddingGuestId {get;set;}

        [Required]
        public int AttendantId {get;set;}

        public User Attendant {get;set;}

        [Required]
        public int MarriageId {get;set;}

        public Wedding Marriage {get;set;}
    }
}