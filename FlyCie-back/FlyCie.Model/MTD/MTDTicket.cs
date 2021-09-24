namespace FlyCie.Model.MTD
{
    public class MTDTicket
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserType UserType { get; set; }
        public bool IsPaid { get; set; }
        public uint NbAdditionalLuggage { get; set; }
    }
}
