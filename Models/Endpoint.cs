using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using FhirDeathRecord;
using Bogus;
using Bogus.Extensions.UnitedStates;
using Hl7.Fhir.ElementModel;
using Hl7.Fhir.Introspection;
using Hl7.Fhir.Serialization;
using Hl7.Fhir.Specification;
using Hl7.Fhir.Utility;
using Hl7.Fhir.Model;
using Hl7.FhirPath;

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
