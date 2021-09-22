using System.Collections.Generic;

namespace FlyCie.Model
{
    public class TicketForm
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
        public bool LoungeSupplement { get; set; }
        public IEnumerable<string> FlightIds { get; set; }
        public string Currency { get; set; }
    }
}
