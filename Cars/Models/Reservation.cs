using System;
using Cars.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cars.Models
{
    public class Reservation
    {
        [Key]
        public int ReservationId { get; set; }

        [ForeignKey("CarId")]
        public int CarId { get; set; }
        public virtual Car? Car { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string CustomerName { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime ReservationDate { get; set; }

        [Column(TypeName = "datetime")]
        [CustomValidation(typeof(Reservation), nameof(ValidatePickupDate))]
        public DateTime PickupDate { get; set; }

        [Column(TypeName = "datetime")]
        [CustomValidation(typeof(Reservation), nameof(ValidateReturnDate))]
        public DateTime ReturnDate { get; set; }

        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }

        public static ValidationResult ValidatePickupDate(DateTime pickupDate)
        {
            if (pickupDate < DateTime.Now)
            {
                return new ValidationResult("Pickup date must be in the future.");
            }
            return ValidationResult.Success;
        }

        public static ValidationResult ValidateReturnDate(DateTime returnDate, ValidationContext context)
        {
            var instance = context.ObjectInstance as Reservation;
            if (instance != null && returnDate <= instance.PickupDate)
            {
                return new ValidationResult("Return date must be later than pickup date.");
            }
            return ValidationResult.Success;
        }
    }
}
