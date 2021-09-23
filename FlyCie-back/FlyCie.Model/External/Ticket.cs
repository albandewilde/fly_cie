using System;
using System.Collections.Generic;
using System.Text;

namespace FlyCie.Model.External
{
    public class Ticket
    {
        public string code { get; set; }
        public Flight flight { get; set; }
        public string date { get; set; }
        public int payed_price { get; set; }
        public string customer_name { get; set; }
        public string customer_nationality { get; set; }
        public string booking_source { get; set; }
        public List<FlightOptions>? options { get; set; }
    }
}
