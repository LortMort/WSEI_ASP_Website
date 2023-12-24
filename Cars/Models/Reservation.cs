using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cars.Models
{
    // Reservation Entity
    public class Reservation
    {
        [Key]
        // Unique identifier for each reservation
        public int ReservationId { get; set; }

        // Foreign key referencing the associated car
        [ForeignKey("Car")]
        public int CarId { get; set; }

        public Car Car { get; set; }

        // Name of the customer making the reservation
        [Column(TypeName = "varchar(255)")]
        public string CustomerName { get; set; }

        // Date and time when the reservation was made
        [Column(TypeName = "datetime")]
        public DateTime ReservationDate { get; set; }

        // Date and time when the car will be picked up
        [Column(TypeName = "datetime")]
        public DateTime PickupDate { get; set; }

        // Date and time when the car is expected to be returned
        [Column(TypeName = "datetime")]
        public DateTime ReturnDate { get; set; }

    }
}
