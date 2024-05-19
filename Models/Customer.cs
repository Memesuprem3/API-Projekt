using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [Required]
        [MaxLength(30)]
        public string FristName { get; set; }
        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }

        public string Adress { get; set; }
        [Required]
        [MaxLength(30)]
        public string Phone { get; set; }
        [Required]
        [MaxLength(30)]
        public string Email { get; set; }
        //many to manny
        public ICollection<Appointment> Appointments { get; set; }
    }
}
