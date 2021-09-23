using System.Collections.Generic;

namespace FlyCie.Model
{
    public class FlightOrder
    {
        public string Code { get; set; }
        public IEnumerable<string>? Options { get; set; }
    }
}
