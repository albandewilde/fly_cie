namespace FlyCie.Model
{
    public class Flight
    {
        public string flightCode { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public int availablePlaces { get; set; }
        public int totalPlaces { get; set; }
        public double price { get; set; }
    }
}
