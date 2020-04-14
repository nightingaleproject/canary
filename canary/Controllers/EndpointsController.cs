using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VRDR;
using canary.Models;

namespace canary.Controllers
{
    [ApiController]
    public class EndpointsController : ControllerBase
    {

        /// <summary>
        /// Creates a new endpoint. Returns its id.
        /// GET /api/endpoints/new
        /// </summary>
        [HttpGet("Endpoints/New")]
        public int New()
        {
            // Find the record in the database and return it
            using (var db = new RecordContext())
            {
                if (db.Endpoints.Count() > 100)
                {
                    // Too many endpoints in existance, delete the oldest to prevent someone from abusing this.
                    // TODO: Probably a smoother way to accomplish this. Investigate.
                    db.Endpoints.Remove(db.Endpoints.FirstOrDefault());
                }
                Endpoint endpoint = new Endpoint();
                db.Endpoints.Add(endpoint);
                db.SaveChanges();
                return endpoint.EndpointId;
            }
        }

        /// <summary>
        /// Given an id, returns the corresponding endpoint.
        /// GET /api/records/{id}
        /// </summary>
        [HttpGet("Endpoints/{id:int}")]
        [HttpGet("Endpoints/Get/{id:int}")]
        public Endpoint Get(int id)
        {
            // Find the record in the database and return it
            using (var db = new RecordContext())
            {
                return db.Endpoints.Where(e => e.EndpointId == id).FirstOrDefault();
            }
        }

        /// <summary>
        /// Lets you post a raw record to Canary, which is processed and added to the Endpoint.
        /// POST /api/endpoints/record/{id:int}
        /// </summary>
        [HttpPost("Endpoints/Record/{id:int}")]
        public async Task<int> RecordPost(int id)
        {
            (Record record, List<Dictionary<string, string>> issues) = (null, null);
            string input;
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                input = await reader.ReadToEndAsync();
            }
            if (!String.IsNullOrEmpty(input))
            {
                if (input.Trim().StartsWith("<") || input.Trim().StartsWith("{")) // XML or JSON?
                {
                    (record, issues) = Record.CheckGet(input, false);
                }
                else
                {
                    try // IJE?
                    {
                        if (input.Length != 5000)
                        {
                            (record, issues) = (null, new List<Dictionary<string, string>> { new Dictionary<string, string> { { "severity", "error" }, { "message", "The given input does not appear to be a valid record." } } });
                        }
                        IJEMortality ije = new IJEMortality(input);
                        DeathRecord deathRecord = ije.ToDeathRecord();
                        (record, issues) = (new Record(deathRecord), new List<Dictionary<string, string>> {} );
                    }
                    catch (Exception e)
                    {
                        (record, issues) = (null, new List<Dictionary<string, string>> { new Dictionary<string, string> { { "severity", "error" }, { "message", e.Message } } });
                    }
                }
                if (record != null)
                {
                    using (var db = new RecordContext())
                    {
                        Endpoint endpoint = db.Endpoints.Where(e => e.EndpointId == id).FirstOrDefault();
                        endpoint.Record = record;
                        endpoint.Issues = issues;
                        endpoint.Finished = true;
                        db.SaveChanges();
                        return endpoint.EndpointId;
                    }
                }
            }
            else
            {
                issues = new List<Dictionary<string, string>> { new Dictionary<string, string> { { "severity", "error" }, { "message", "The given input appears to be empty." } } };
            }

            if (record == null && issues != null)
            {
                using (var db = new RecordContext())
                {
                    Endpoint endpoint = db.Endpoints.Where(e => e.EndpointId == id).FirstOrDefault();
                    endpoint.Issues = issues;
                    endpoint.Finished = true;
                    db.SaveChanges();
                    return endpoint.EndpointId;
                }
            }

            return 0;
        }

    }
}
