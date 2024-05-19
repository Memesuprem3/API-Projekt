using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class BookingHistory
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public DateTime ChangedAt { get; set; }
        public string ChangeType { get; set; }
        public string ChangedBy { get; set; }

        public Appointment Appointment { get; set; }
    }
}
