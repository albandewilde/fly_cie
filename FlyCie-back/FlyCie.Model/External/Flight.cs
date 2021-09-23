using System.Collections.Generic;

namespace FlyCie.Model.External
{
    public class Flight
    {
        public string code { get; set; }
        public string departure { get; set; }
        public string arrival { get; set; }
        public int base_price { get; set; }
        public Plane plane { get; set; }
        public int seats_booked { get; set; }
        public IEnumerable<FlightOptions> options { get; set; }
    }
}
