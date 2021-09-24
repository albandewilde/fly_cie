using System.Collections.Generic;

namespace FlyCie.Model.MTD
{
    public class MTDPayload
    {
        public IEnumerable<Ticket> Tickets { get; set; }
        public string UserType { get; set; }
        public uint NbAdditionalLuggage { get; set; }
    }
}
