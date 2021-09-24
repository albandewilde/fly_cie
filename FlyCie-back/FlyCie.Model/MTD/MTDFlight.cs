namespace FlyCie.Model.MTD
{
    public class MTDFlight
    {
        public string idFlight { get; set; }
        public double basePrice { get; set; }
        public double additionalLuggagePrice { get; set; }
        public string departurePlace { get; set; }
        public string arrivalPlace { get; set; }
        public int availableSeats { get; set; }
    }
}
