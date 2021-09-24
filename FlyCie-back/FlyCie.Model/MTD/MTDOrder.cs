using System.Collections.Generic;

namespace FlyCie.Model.MTD
{
    public class MTDOrder
    {
        public MTDUser User { get; set; }
        public List<MTDTicket> TicketList { get; set; }
        public MTDCurrency UsedCurrency { get; set; }
        public bool IsPaid { get; set; }
    }

    public class MTDUser
    {
        public string Name { get; set; }
    }

    public enum MTDCurrency
    {
        EUR,
        USD
    }
}
