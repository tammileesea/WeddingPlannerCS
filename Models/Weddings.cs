using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models {
    public class FutureDateAttribute : ValidationAttribute {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext){
            if (value == null){
                return new ValidationResult("Wedding date is required");
            }
            if ((DateTime)value <= DateTime.Now) {
                return new ValidationResult("Wedding must take place in future");
            } else {
                return ValidationResult.Success;
            }
        }
    }

    public class Wedding {
        [Key]
        public int WeddingId {get;set;}

        [Required]
        public int CreatorId {get;set;}

        [Required(ErrorMessage = "*Person one's name is required")]
        [MinLength(2, ErrorMessage = "*Person one's name must be at least 2 letters")]
        [Display(Name = "Person One")]
        public string PersonOne {get;set;}

        [Required(ErrorMessage = "*Person two's name is required")]
        [MinLength(2, ErrorMessage = "*Person two's name must be at least 2 letters")]
        [Display(Name = "Person Two")]
        public string PersonTwo {get;set;}

        [FutureDate]
        [Required(ErrorMessage = "*Wedding date is required")]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        // [DisplayFormat(DataFormatString = "{dddd, MMMM dd, yyyy}")]
        public DateTime WeddingDate {get;set;}

        [Required(ErrorMessage = "*Venue address is required")]
        [Display(Name = "Venue Address")]
        public string WeddingAddress {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        public List<WeddingGuest> Guests {get;set;}
    }
}