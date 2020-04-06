using System.Collections.Generic;

namespace canary.Models
{
    public class Endpoint
    {
        public int EndpointId { get; set; }

        public bool Finished { get; set; }

        public Record Record { get; set; }

        public List<Dictionary<string, string>> Issues { get; set; }

        public Endpoint()
        {
            Finished = false;
        }
    }
}
