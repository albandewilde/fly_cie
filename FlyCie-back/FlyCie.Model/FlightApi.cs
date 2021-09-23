using System.Collections.Generic;

namespace FlyCie.Model
{
    public class FlightApi : Flight
    {
        public IEnumerable<Model.External.FlightOptions> Options { get; set; }
        public IDictionary<string, object>? AdditionalFields { get; set; }
    }
}
