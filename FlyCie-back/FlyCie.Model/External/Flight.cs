namespace FlyCie.Model.External
{
    public class Flight
    {
        public string code { get; set; }
        public Airport departure { get; set; }
        public Airport arrival { get; set; }
        public int base_price { get; set; }
        public Plane plane { get; set; }
        public int seats_booked { get; set; }
    }
}
