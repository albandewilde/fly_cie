using System;

namespace FlyCie.Model
{
    public class Ticket
    {
        public Flight Flight { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
        public bool LoungeSupplement { get; set; }
        public Currency Currency { get; set; }
    }
}
