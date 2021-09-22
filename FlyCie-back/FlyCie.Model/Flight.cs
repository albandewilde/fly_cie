namespace FlyCie.Model
{
    public class Flight
    {
        public string FlightCode { get; set; }
        public Airport From { get; set; }
        public Airport To { get; set; }
        public int AvailablePlaces { get; set; }
        public int TotalPlaces { get; set; }
        public double Price { get; set; }
    }
}
