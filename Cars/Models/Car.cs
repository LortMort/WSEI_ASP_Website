using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Cars.Models
{
    public enum CarTypes{
        Sedan = 1,
        Hatchback = 2,
        Coupe = 3,
        Cabriolet = 4,
        SUV = 5
    }

    public class Car
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Brand { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Model { get; set; }

        [Column(TypeName = "varchar(50)")]
        public CarTypes Type { get; set; }

        [Column(TypeName = "int")]
        public int Power { get; set; }

        [Column(TypeName = "bit")]
        public bool Avalibity { get; set; }
    }
}