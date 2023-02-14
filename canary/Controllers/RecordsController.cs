using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VRDR;
using canary.Models;

namespace canary.Controllers
{
    [ApiController]
    public class RecordsController : ControllerBase
    {
        /// <summary>
        /// Returns all records.
        /// GET /api/records
        /// </summary>
        [HttpGet("Records")]
        [HttpGet("Records/Index")]
        public Record[] Index()
        {
            // Find the record in the database and return it
            using (var db = new RecordContext())
            {
                return db.Records.ToArray();
            }
        }

        /// <summary>
        /// Given an id, returns the corresponding record.
        /// GET /api/records/{id}
        /// </summary>
        [HttpGet("Records/{id:int}")]
        [HttpGet("Records/Get/{id:int}")]
        public Record Get(int id)
        {
            // Find the record in the database and return it
            using (var db = new RecordContext())
            {
                return db.Records.Where(record => record.RecordId == id).FirstOrDefault();
            }
        }

        /// <summary>
        /// Given an id, returns the corresponding (death) record as JSON.
        /// GET /api/records/json/{id}
        /// </summary>
        [HttpGet("Records/JSON/{id:int}")]
        public string GetJson(int id)
        {
            return Connectathon.FromId(id).ToJSON();
        }

        /// <summary>
        /// Given an id, returns the corresponding (death) record as XML.
        /// GET /api/records/xml/{id}
        /// </summary>
        [HttpGet("Records/XML/{id:int}")]
        public string Getxml(int id)
        {
            return Connectathon.FromId(id).ToXML();
        }

        /// <summary>
        /// Creates a new synthetic death record, and returns it. Does not save it to the database.
        /// GET /api/records/new
        /// </summary>
        [HttpGet("Records/New")]
        public Record NewGet(string state, string type, string sex)
        {
            // Create new record from scratch
            Record record = new Record();

            // Populate the record with fake data
            record.Populate(state, type, sex);

            // Return the record
            return record;
        }

        /// <summary>
        /// Creates a new death record using the contents provided. Returns the record and any validation issues.
        /// POST /api/records/new
        /// </summary>
        [HttpPost("Records/New")]
        public async Task<(Record record, List<Dictionary<string, string>> issues)> NewPost()
        {
            string input = await new StreamReader(Request.Body, Encoding.UTF8).ReadToEndAsync();

            if (!String.IsNullOrEmpty(input))
            {
                if (input.Trim().StartsWith("<") || input.Trim().StartsWith("{")) // XML or JSON?
                {
                    return Record.CheckGet(input, false);
                }
                else
                {
                    try // IJE?
                    {
                        // If input.Length != 5000, truncate/pad according to force it to 5000.
                        if (input.Length > 5000)
                        {
                            input = input.Substring(0, 5000);
                        }
                        else if (input.Length < 5000)
                        {
                            input = input.PadRight(5000, ' ');
                        }
                        IJEMortality ije = new IJEMortality(input);
                        DeathRecord deathRecord = ije.ToDeathRecord();
                        return (new Record(deathRecord), new List<Dictionary<string, string>> {} );
                    }
                    catch (Exception e)
                    {
                        String message = e.Message;
                        while (e.InnerException != null)
                        {
                            e = e.InnerException;
                            message += "; Inner Exception = [ " + e.Message + " ]";
                        }
                        return (null, new List<Dictionary<string, string>> { new Dictionary<string, string> { { "severity", "error" }, { "message", message } } });
                    }
                }
            }
            else
            {
                return (null, new List<Dictionary<string, string>> { new Dictionary<string, string> { { "severity", "error" }, { "message", "The given input appears to be empty." } } });
            }
        }

        /// <summary>
        /// Creates a new death record using the "description" contents provided. Returns the record.
        /// POST /api/records/description/new
        /// </summary>
        [HttpPost("Records/Description/New")]
        public async Task<Record> NewDescriptionPost()
        {
            string input = await new StreamReader(Request.Body, Encoding.UTF8).ReadToEndAsync();

            if (!String.IsNullOrEmpty(input))
            {
                DeathRecord record = DeathRecord.FromDescription(input);
                return new Record(record);
            }
            return null;
        }

        /// <summary>
        /// Get's the DeathRecord "Description" structure.
        /// GET /api/records/description
        /// </summary>
        [HttpGet("Records/Description")]
        public string GetDescription()
        {
            DeathRecord record = new DeathRecord();
            Dictionary<string, Dictionary<string, dynamic>> description = new Dictionary<string, Dictionary<string, dynamic>>();
            foreach(PropertyInfo property in typeof(DeathRecord).GetProperties().OrderBy(p => p.GetCustomAttribute<Property>().Priority))
            {
                // Grab property annotation for this property
                Property info = property.GetCustomAttribute<Property>();

                // Skip properties that shouldn't be serialized.
                if (!info.Serialize)
                {
                    continue;
                }

                // Add category if it doesn't yet exist
                if (!description.ContainsKey(info.Category))
                {
                    description.Add(info.Category, new Dictionary<string, dynamic>());
                }

                // Add the new property to the category
                Dictionary<string, dynamic> category = description[info.Category];
                category[property.Name] = new Dictionary<string, dynamic>();

                // Add the attributes of the property
                category[property.Name]["Name"] = info.Name;
                category[property.Name]["Type"] = info.Type.ToString();
                category[property.Name]["Description"] = info.Description;

                // Add the current value of the property
                if (info.Type == Property.Types.Dictionary)
                {
                    // Special case for Dictionary; we want to be able to describe what each key means
                    Dictionary<string, string> value = (Dictionary<string, string>)property.GetValue(record);
                    if (value == null)
                    {
                        continue;
                    }
                    Dictionary<string, Dictionary<string, string>> moreInfo = new Dictionary<string, Dictionary<string, string>>();
                    foreach (PropertyParam parameter in property.GetCustomAttributes<PropertyParam>())
                    {
                        moreInfo[parameter.Key] = new Dictionary<string, string>();
                        moreInfo[parameter.Key]["Description"] = parameter.Description;
                        moreInfo[parameter.Key]["Value"] = null;
                        if (value.ContainsKey(parameter.Key))
                        {
                            moreInfo[parameter.Key]["Value"] = value[parameter.Key];
                        }
                        else
                        {
                            moreInfo[parameter.Key]["Value"] = null;
                        }

                    }
                    category[property.Name]["Value"] = moreInfo;
                }
                else
                {
                    category[property.Name]["Value"] = property.GetValue(record);
                }
            }
            return JsonConvert.SerializeObject(description);
        }

        /// <summary>
        /// Creates a new synthetic death record, saves it to the database, and returns it.
        /// GET /api/records/new
        /// </summary>
        [HttpGet("Records/Create")]
        public Record Create(string state, string type)
        {
            //using (var db = new RecordContext())
            //{
                // Create new record from scratch
                Record record = new Record();

                // Populate the record with fake data
                record.Populate();

                // Save the record to the database
                //db.Records.Add(record);
                //db.SaveChanges();

                // Return the record
                return record;
            //}
        }
    }
}
