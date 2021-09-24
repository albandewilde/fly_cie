namespace FlyCie.Model.MTD
{
    public class MTDFlight
    {
        public string IdFlight { get; set; }
        public double BasePrice { get; set; }
        public double AdditionalLuggagePrice { get; set; }
        public string DeparturePlace { get; set; }
        public string ArrivalPlace { get; set; }
        public int AvailableSeats { get; set; }
    }
}
